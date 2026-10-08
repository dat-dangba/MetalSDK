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

        public bool IsReady { get; private set; }

        protected abstract ServiceContainer Container { get; }

#if UNITY_EDITOR

        private bool IsService(MonoBehaviour mb) => mb is IService;

        protected virtual void LoadAllService()
        {
            _Services = new List<MonoBehaviour>();
            if (transform.childCount <= 0) return;
            foreach (Transform item in transform)
            {
                _Services.Add(item.gameObject.GetComponent<MonoBehaviour>());
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
            RegisterServices();
            await BootstrapAsync();
        }

        private void Update()
        {
            if (!IsReady) return;
            Container.TickAll();
        }

        protected virtual void RegisterServices()
        {
            foreach (var service in _Services)
            {
                RegisterService(service);
            }
        }

        protected virtual void RegisterService(MonoBehaviour service)
        {
            var type = service.GetType();
            var method = typeof(ServiceContainer).GetMethod(nameof(ServiceContainer.Register));
            if (method == null) return;
            var generic = method.MakeGenericMethod(type);
            generic.Invoke(Container, new object[] { service });
        }

        // protected virtual void RegisterService(IService service)
        // {
        //     var type = service.GetType();
        //     var interfaces = GetMostDerivedInterfaces(type);
        //     var method = typeof(ServiceContainer).GetMethod(nameof(ServiceContainer.Register));
        //     if (method == null) return;
        //     var generic = method.MakeGenericMethod(interfaces.Length == 0 ? type : interfaces[0]);
        //     generic.Invoke(Container, new object[] { service });
        // }
        //
        // private Type[] GetMostDerivedInterfaces(Type concreteType)
        // {
        //     var allInterfaces = concreteType.GetInterfaces();
        //     // Loại bỏ interface nào là "cha" của 1 interface khác trong cùng danh sách
        //     return allInterfaces
        //         .Where(candidate => !allInterfaces.Any(other =>
        //             other != candidate && candidate.IsAssignableFrom(other)))
        //         .ToArray();
        // }

        protected virtual async Task BootstrapAsync()
        {
            InitializeServices();

            await AsyncInitializeServices();
            IsReady = true;
#if UNITY_EDITOR
            Debug.LogWarning($"[{GetType().Name}] - Ready {IsReady}");
#endif
        }

        protected virtual async Task AsyncInitializeServices()
        {
            List<int> orders = Container.All()
                .OfType<IAsyncInitializable>()
                .Select(s => s.OrderInBootstrap)
                .Distinct()
                .OrderBy(o => o)
                .ToList();

            foreach (var item in orders)
            {
                var asyncTasks = new List<Task>();
                foreach (var sync in Container.All().OfType<IAsyncInitializable>())
                {
                    if (sync.OrderInBootstrap == item)
                    {
                        asyncTasks.Add(sync.InitializeAsync());
                    }
                }

                await Task.WhenAll(asyncTasks);
            }
        }

        protected virtual void InitializeServices()
        {
            foreach (var sync in Container.All().OfType<IInitializable>())
            {
                sync.Initialize();
            }
        }
    }
}
