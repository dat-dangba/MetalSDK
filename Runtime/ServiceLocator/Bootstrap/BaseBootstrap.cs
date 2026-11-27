using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
#if HAS_AUTO_REFERENCE
using Teo.AutoReference;
#endif

namespace Metal
{
    [DefaultExecutionOrder(-1)]
    public abstract class BaseBootstrap : MonoBehaviour
    {
#if HAS_AUTO_REFERENCE
        [SerializeField, GetInChildren, IgnoreSelf, FilterBy(nameof(IsService))]
#else
        [SerializeField]
#endif
        protected List<MonoBehaviour> _Services;

        /// <summary>
        /// Service do chính bootstrap này register (chỉ những service này được Initialize / tick bởi bootstrap)
        /// </summary>
        private readonly List<IService> _registeredServices = new();

        public bool IsReady { get; private set; }

        protected abstract ServiceContainer Container { get; }

        /// <summary>
        /// Thời gian tối đa chờ 1 IAsyncInitializable. Quá thời gian thì bootstrap chạy tiếp (service vẫn chạy nền).
        /// 0 = chờ không giới hạn.
        /// </summary>
        protected virtual int AsyncInitializeTimeoutMs => 15000;

#if HAS_AUTO_REFERENCE
        private bool IsService(MonoBehaviour mb) => mb is IService;
#endif

#if UNITY_EDITOR
        protected virtual void LoadAllService()
        {
            _Services = new List<MonoBehaviour>();
            if (transform.childCount <= 0) return;
            foreach (Transform item in transform)
            {
                foreach (MonoBehaviour component in item.GetComponents<MonoBehaviour>())
                {
                    if (component is IService)
                    {
                        _Services.Add(component);
                    }
                }
            }
        }

        private void Reset()
        {
#if HAS_AUTO_REFERENCE
            AutoReference.Sync(this);
#else
            LoadAllService();
#endif
        }
#endif

        protected virtual async void Awake()
        {
            DontDestroyOnLoad(gameObject);
            try
            {
                RegisterServices();
                await BootstrapAsync();
            }
            catch (Exception ex)
            {
                MetalLog.LogError($"[{GetType().Name}] Bootstrap failed: {ex}", true);
                IsReady = true;
            }

            MergeBootstrap();
        }

        protected virtual void MergeBootstrap()
        {
        }

        private void Update()
        {
            if (!IsReady) return;
            Container.TickAll();
        }

        protected virtual void RegisterServices()
        {
            if (_Services == null) return;

            foreach (var service in _Services)
            {
                if (service == null)
                {
                    MetalLog.LogWarning($"[{GetType().Name}] Có phần tử null trong _Services, bỏ qua", true);
                    continue;
                }

                RegisterService(service);
            }
        }

        protected virtual void RegisterService(MonoBehaviour service)
        {
            if (service is not IService iService)
            {
                MetalLog.LogWarning($"[{GetType().Name}] {service.GetType().Name} không phải IService, bỏ qua", true);
                return;
            }

            // Register theo kiểu cụ thể, chưa tick cho tới khi bootstrap khởi tạo xong
            Container.Register(service.GetType(), service, false);
            _registeredServices.Add(iService);
        }

        protected virtual async Task BootstrapAsync()
        {
            InitializeServices();
            await AsyncInitializeServices();

            foreach (IService service in _registeredServices)
            {
                Container.StartTicking(service);
            }

            IsReady = true;
#if UNITY_EDITOR
            Debug.LogWarning($"[{GetType().Name}] - Ready {IsReady}");
#endif
        }

        /// <summary>
        /// Chạy InitializeAsync theo từng nhóm OrderInBootstrap tăng dần, các service cùng nhóm chạy song song.
        /// Service lỗi / quá thời gian chỉ log lỗi, không chặn bootstrap.
        /// </summary>
        protected virtual async Task AsyncInitializeServices()
        {
            List<IGrouping<int, IAsyncInitializable>> groups = _registeredServices
                .OfType<IAsyncInitializable>()
                .GroupBy(s => s.OrderInBootstrap)
                .OrderBy(g => g.Key)
                .ToList();

            foreach (IGrouping<int, IAsyncInitializable> group in groups)
            {
                await Task.WhenAll(group.Select(InitializeAsyncSafe));
            }
        }

        protected virtual void InitializeServices()
        {
            foreach (IInitializable service in _registeredServices.OfType<IInitializable>())
            {
                try
                {
                    service.Initialize();
                }
                catch (Exception ex)
                {
                    MetalLog.LogError($"[{GetType().Name}] {service.GetType().Name}.Initialize failed: {ex}", true);
                }
            }
        }

        private async Task InitializeAsyncSafe(IAsyncInitializable service)
        {
            string serviceName = service.GetType().Name;
            try
            {
                Task initTask = service.InitializeAsync() ?? Task.CompletedTask;
                if (AsyncInitializeTimeoutMs <= 0)
                {
                    await initTask;
                    return;
                }

                Task completed = await Task.WhenAny(initTask, Task.Delay(AsyncInitializeTimeoutMs));
                if (completed != initTask)
                {
                    MetalLog.LogError(
                        $"[{GetType().Name}] {serviceName}.InitializeAsync timeout {AsyncInitializeTimeoutMs}ms, continue bootstrap",
                        true);
                    ObserveLater(initTask, serviceName);
                    return;
                }

                await initTask;
            }
            catch (Exception ex)
            {
                MetalLog.LogError($"[{GetType().Name}] {serviceName}.InitializeAsync failed: {ex}", true);
            }
        }

        /// <summary>
        /// Service quá thời gian vẫn chạy nền: log lỗi nếu sau đó nó throw
        /// </summary>
        private void ObserveLater(Task task, string serviceName)
        {
            task.ContinueWith(t =>
            {
                MetalLog.LogError($"[{GetType().Name}] {serviceName}.InitializeAsync failed: {t.Exception}", true);
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
