using UnityEditor;
using UnityEngine.UIElements;

namespace Metal.Editor
{
    public class MetalAdsVisualElement : BaseVisualElement
    {
        public MetalAdsVisualElement()
        {
#if !HAS_MAX
            Add(new MaxVisualElement());
#elif !HAS_METAL_ADS
            Add(new InstallPackageVisualElement("Metal Ads Sdk", InstallMetalAds));
#else
            DrawSetting<MetalAdsSetting>();
#endif
        }

        private void InstallMetalAds()
        {
            string token = MetalServicesEditor.GetToken();
            if (string.IsNullOrEmpty(token)) return;
            InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/MetalAds.git");
        }
    }
}
