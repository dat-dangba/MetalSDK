using Metal.Analytics;
using UnityEngine.UIElements;

namespace Metal.Editor
{
    public class MetalAnalyticsVisualElement : BaseVisualElement
    {
        public MetalAnalyticsVisualElement()
        {
#if HAS_METAL_ANALYTICS
            Add(new PackageInstalledVisualElement("Metal Analytics Sdk"));
            Add(new VisualElement
            {
                style =
                {
                    marginTop = 10
                }
            });
            DrawSetting(MetalAnalyticsBuildConfig.LoadConfig());
#elif !HAS_ADJUST
            Add(new AdjustVisualElement());
#else
            Add(new InstallPackageVisualElement("Metal Analytics Sdk", InstallMetalAnalyticsSDK));
#endif
        }

        private void InstallMetalAnalyticsSDK()
        {
            string token = MetalServicesEditor.GetToken();
            if (string.IsNullOrEmpty(token)) return;
            InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/MetalAnalytics.git");
        }
    }
}
