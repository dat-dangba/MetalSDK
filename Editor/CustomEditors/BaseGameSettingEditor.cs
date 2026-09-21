using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Metal.Editor
{
    [CustomEditor(typeof(BaseGameSetting))]
    public class BaseGameSettingEditor : MetalSettingEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

#if HAS_AUTO_REFERENCE
            InstallBaseGame(root);
#else
            root.Add(new InstallPackageVisualElement("Auto Reference", InstallAutoReference));
#endif

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            HideScript(root);

            return root;
        }

        private void InstallBaseGame(VisualElement root)
        {
#if HAS_METAL_BASE_GAME
            root.Add(new PackageInstalledVisualElement("Base Game"));

            Button createProjectStructure = new Button(CreateProjectStructure.CopyProjectStructure)
            {
                text = "Create Project Structure",
                style =
                {
                    marginTop = 50
                }
            };
#else
            root.Add(new InstallPackageVisualElement("Base Game", InstallBaseGame));
#endif
        }

        private void InstallAutoReference()
        {
            InstallPackageHelper.Install($"https://github.com/dat-dangba/AutoReference.git");
        }

        private void InstallCore()
        {
            string token = MetalServicesEditor.GetToken();
            if (string.IsNullOrEmpty(token)) return;

            InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/MetalCore.git");
        }

        private void InstallBaseGame()
        {
            string token = MetalServicesEditor.GetToken();
            if (string.IsNullOrEmpty(token)) return;

            InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/MetalBaseGame.git");
        }
    }
}
