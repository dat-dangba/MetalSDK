namespace Metal
{
    public class SdkBootstrap : BaseBootstrap
    {
        private static SdkBootstrap Instance;

        protected override ServiceContainer Container => ServiceLocator.Sdk;

        protected override void Awake()
        {
            // Load lại scene bootstrap: chỉ giữ SdkBootstrap đầu tiên, không register / khởi tạo SDK lần 2
            if (Instance != null && Instance != this)
            {
                MetalLog.LogWarning("[SdkBootstrap] Đã có SdkBootstrap, huỷ bản trùng");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            base.Awake();
        }
    }
}
