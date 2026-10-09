#if HAS_METAL_GAME_ANALYTICS
using Metal.GameAnalytics;
#endif
using UnityEngine.UIElements;

namespace Metal.Editor
{
    public class MetalGameAnalyticsVisualElement : BaseVisualElement
    {
        public MetalGameAnalyticsVisualElement()
        {
#if HAS_METAL_GAME_ANALYTICS
            Add(new PackageInstalledVisualElement("Metal Game Analytics Sdk"));
            Add(new VisualElement
            {
                style =
                {
                    marginTop = 10
                }
            });
            DrawSetting(MetalAnalyticsBuildConfig.Load());
#elif !HAS_ADJUST
            Add(new AdjustVisualElement());
#else
            Add(new InstallPackageVisualElement("Metal Game Analytics Sdk", InstallMetalAnalyticsSDK));
#endif
        }

        private void InstallMetalAnalyticsSDK()
        {
            string token = MetalSDK.GetToken();
            if (string.IsNullOrEmpty(token)) return;
            InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/MetalGameAnalytics.git");
        }
    }
}
