using System;
using System.Collections.Generic;
using UnityEngine;

namespace Metal
{
    public sealed class ServiceContainer
    {
        private readonly Dictionary<Type, object> _services = new();
        private readonly List<ITickable> _tickables = new();
        private readonly object _lock = new();

        // Bản sao của _tickables để TickAll không lỗi khi register / unregister ngay trong Tick, chỉ tạo lại khi thay đổi
        private ITickable[] _tickSnapshot = Array.Empty<ITickable>();
        private bool _isTickSnapshotDirty;

        public void Register<T>(T service) where T : class
        {
            Register(typeof(T), service, true);
        }

        /// <summary>
        /// Register theo type truyền vào. startTicking = false: chưa tick, bootstrap gọi StartTicking sau khi
        /// service khởi tạo xong.
        /// </summary>
        internal void Register(Type type, object service, bool startTicking)
        {
            lock (_lock)
            {
                if (_services.TryGetValue(type, out object old))
                {
#if UNITY_EDITOR
                    Debug.LogWarning($"[ServiceContainer] - {type.Name} đã được register, sẽ ghi đè.");
#endif
                    RemoveTickable(old);
                }

                _services[type] = service;

                if (startTicking)
                {
                    AddTickable(service);
                }
            }
        }

        /// <summary>
        /// Bắt đầu tick service (nếu là ITickable và đang được register)
        /// </summary>
        internal void StartTicking(object service)
        {
            lock (_lock)
            {
                if (!_services.ContainsValue(service)) return;
                AddTickable(service);
            }
        }

        public T Get<T>() where T : class, IService
        {
            lock (_lock)
            {
                if (_services.TryGetValue(typeof(T), out object service))
                {
                    return (T)service;
                }
            }

            throw new InvalidOperationException($"[ServiceContainer] - Service {typeof(T)} chưa được register.");
        }

        public bool TryGet<T>(out T service) where T : class, IService
        {
            lock (_lock)
            {
                if (_services.TryGetValue(typeof(T), out object raw))
                {
                    service = (T)raw;
                    return true;
                }
            }

            service = null;
            return false;
        }

        public bool IsRegistered<T>() where T : class, IService
        {
            lock (_lock)
            {
                return _services.ContainsKey(typeof(T));
            }
        }

        public void Unregister<T>() where T : class, IService
        {
            lock (_lock)
            {
                if (_services.Remove(typeof(T), out object service))
                {
                    RemoveTickable(service);
                }
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                foreach (KeyValuePair<Type, object> kv in _services)
                {
                    DisposeService(kv.Value);
                }

                _services.Clear();
                _tickables.Clear();
                _isTickSnapshotDirty = true;
            }
        }

        public void TickAll()
        {
            ITickable[] tickables;
            lock (_lock)
            {
                if (_isTickSnapshotDirty)
                {
                    _tickSnapshot = _tickables.ToArray();
                    _isTickSnapshotDirty = false;
                }

                tickables = _tickSnapshot;
            }

            foreach (ITickable tickable in tickables)
            {
                try
                {
                    tickable.Tick();
                }
                catch (Exception ex)
                {
                    // 1 service lỗi không chặn tick của các service khác
                    Debug.LogException(ex);
                }
            }
        }

        /// <summary>
        /// Bản sao danh sách service đang register
        /// </summary>
        public IReadOnlyCollection<object> All()
        {
            lock (_lock)
            {
                return new List<object>(_services.Values);
            }
        }

        private void AddTickable(object service)
        {
            if (service is not ITickable tickable || _tickables.Contains(tickable)) return;

            _tickables.Add(tickable);
            _isTickSnapshotDirty = true;
        }

        private void RemoveTickable(object service)
        {
            if (service is ITickable tickable && _tickables.Remove(tickable))
            {
                _isTickSnapshotDirty = true;
            }
        }

        private static void DisposeService(object service)
        {
            try
            {
                // Hỗ trợ cả Metal.IDisposable lẫn System.IDisposable
                if (service is IDisposable metalDisposable)
                {
                    metalDisposable.Dispose();
                }
                else if (service is System.IDisposable systemDisposable)
                {
                    systemDisposable.Dispose();
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
