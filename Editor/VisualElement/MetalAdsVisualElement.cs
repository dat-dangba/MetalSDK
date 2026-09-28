#if HAS_METAL_ADS
using Metal.Ads;
#endif
using UnityEngine.UIElements;

namespace Metal.Editor
{
    public class MetalAdsVisualElement : BaseVisualElement
    {
        public MetalAdsVisualElement()
        {
#if HAS_MAX && HAS_GOOGLE_ADS
#if !HAS_METAL_ADS
            Add(new InstallPackageVisualElement("Metal Ads Sdk", InstallMetalAds));
#else
            Add(new PackageInstalledVisualElement("Metal Ads Sdk"));
            Add(new VisualElement()
            {
                style =
                {
                    marginTop = 10
                }
            });
            DrawSetting(MetalAdsBuildConfig.Load());
#endif
#else
#if !HAS_MAX
            Add(new MaxVisualElement());
#else
            Add(new PackageInstalledVisualElement("MAX"));
#endif
#if !HAS_GOOGLE_ADS
            Add(new VisualElement()
            {
                style =
                {
                    marginTop = 10
                }
            });
            Add(new GoogleAdsVisualElement());
#else
            Add(new VisualElement()
            {
                style =
                {
                    marginTop = 10
                }
            });
            Add(new PackageInstalledVisualElement("Google Ads"));
#endif
#endif
        }

        private void InstallMetalAds()
        {
            string token = MetalSDK.GetToken();
            if (string.IsNullOrEmpty(token)) return;
            InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/MetalAds.git");
        }
    }
}
