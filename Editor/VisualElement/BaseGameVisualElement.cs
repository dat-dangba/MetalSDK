using UnityEngine.UIElements;

namespace Metal.Editor
{
    public class BaseGameVisualElement : BaseVisualElement
    {
        public BaseGameVisualElement()
        {
#if !HAS_AUTO_REFERENCE
            Add(new InstallPackageVisualElement("Auto Reference", InstallAutoReference));
#elif !HAS_METAL_BASE_GAME
            Add(new PackageInstalledVisualElement("Auto Reference"));
            Add(new InstallPackageVisualElement("Base Game", InstallBaseGame)
            {
                style =
                {
                    marginTop = 10
                }
            });
#else
            Add(new PackageInstalledVisualElement("Base Game"));
            Button createProjectStructure = new Button(CreateProjectStructure.CopyProjectStructure)
            {
                text = "Create Project Structure",
                style =
                {
                    marginTop = 10
                }
            };
            Add(createProjectStructure);
#endif
        }

        private void InstallAutoReference()
        {
            InstallPackageHelper.Install($"https://github.com/dat-dangba/AutoReference.git");
        }

        private void InstallBaseGame()
        {
            string token = MetalSDK.GetToken();
            if (string.IsNullOrEmpty(token)) return;

            InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/MetalBaseGame.git");
        }
    }
}
