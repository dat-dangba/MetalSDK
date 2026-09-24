#if HAS_METAL_IAP
using Metal.IAP;
#endif

namespace Metal.Editor
{
    public class MetalInAppPurchaseVisualElement : BaseVisualElement
    {
        public MetalInAppPurchaseVisualElement()
        {
#if !HAS_IN_APP_PURCHASING
            Add(new InstallPackageVisualElement("In app purchasing", InstallInAppPurchasing));
#elif !HAS_METAL_IAP
            Add(new PackageInstalledVisualElement("In app purchasing"));
            Add(new InstallPackageVisualElement("Metal IAP Sdk", InstallMetalIAP)
            {
                style =
                {
                    marginTop = 10
                }
            });
#else
            DrawSetting(InAppPurchaseSo.LoadConfig());
#endif
        }

        private void InstallInAppPurchasing()
        {
            InstallPackageHelper.Install(PackageConstant.IAP_PACKAGE_ID);
        }

        private void InstallMetalIAP()
        {
            string token = MetalServicesEditor.GetToken();
            if (string.IsNullOrEmpty(token)) return;
            string metalIAPLink = $"https://{token}@github.com/dat-dangba/MetalIAP.git";
            InstallPackageHelper.Install(metalIAPLink);
        }
    }
}
