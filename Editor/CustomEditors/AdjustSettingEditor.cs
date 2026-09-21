using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Metal.Editor
{
    [CustomEditor(typeof(AdjustSetting))]
    public class AdjustSettingEditor : MetalSettingEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            if (InstallPackageHelper.IsPackageInstalled(PackageConstant.ADJUST_PACKAGE_ID))
            {
                root.Add(new PackageInstalledVisualElement("Adjust"));
                InspectorElement.FillDefaultInspector(root, serializedObject, this);
#if HAS_METAL_ANALYTICS
                var buildConfig = MetalServicesEditor.GetBuildConfig<AdjustBuildConfig>();
                if (buildConfig != null)
                {
                    var serializedConfig = new SerializedObject(buildConfig);
                    var configInspector = new InspectorElement(serializedConfig)
                    {
                        style =
                        {
                            paddingLeft = 0,
                            marginTop = 10
                        }
                    };
                    root.Add(configInspector);
                }
#endif
            }
            else
            {
                root.Add(new InstallPackageVisualElement("Adjust", InstallAdjust));
            }

            HideScript(root);

            return root;
        }

        private void InstallAdjust()
        {
            InstallPackageHelper.Install(PackageConstant.ADJUST_LINK_INSTALL, () => { });
        }
    }
}
