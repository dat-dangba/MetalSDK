using System.Collections.Generic;
using Metal.IAP;
using UnityEngine.UIElements;

namespace Metal.Editor
{
    public class MetalInAppPurchaseVisualElement : BaseVisualElement
    {
        public MetalInAppPurchaseVisualElement()
        {
#if HAS_METAL_IAP
            DrawSetting(InAppPurchaseSo.LoadConfig());
#else
            Add(new InstallPackageVisualElement("Metal IAP Sdk", InstallMetalIAP));
#endif
        }

        private void InstallMetalIAP()
        {
            string token = MetalServicesEditor.GetToken();
            if (string.IsNullOrEmpty(token)) return;
            var packages = new List<string>();

            if (!InstallPackageHelper.IsPackageInstalled(PackageConstant.IAP_PACKAGE_ID))
            {
                packages.Add(PackageConstant.IAP_PACKAGE_ID);
            }

            string metalIAPLink = $"https://{token}@github.com/dat-dangba/MetalIAP.git";
            if (!InstallPackageHelper.IsPackageInstalled(metalIAPLink))
            {
                packages.Add(metalIAPLink);
            }

            InstallPackageHelper.Install(packages);
        }
    }
}
