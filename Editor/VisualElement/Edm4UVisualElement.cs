using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Metal.Editor
{
    public class Edm4UVisualElement : VisualElement
    {
        public Edm4UVisualElement()
        {
            Add(new InstallPackageVisualElement("EDM4U", InstallEdm4U));
        }

        private void InstallEdm4U()
        {
            RegistryHelper.AddRegistryEdm4U();
            InstallPackageHelper.Install($"{PackageConstant.EDM4U_PACKAGE_ID}@{PackageConstant.EDM4U_VERSION}",
                () =>
                {
                    SessionState.SetString("tab", "General");
                    IntegrationManagerWindow.CloseWindow();
                    IntegrationManagerWindow.ShowWindow();
                });
        }
    }
}
