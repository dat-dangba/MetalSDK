using UnityEditor;
using UnityEngine.UIElements;

namespace Metal.Editor
{
    public class MaxVisualElement : BaseVisualElement
    {
        public MaxVisualElement()
        {
#if HAS_MAX
            DrawSetting<MaxSetting>();
#else
            Add(new InstallPackageVisualElement("MAX Sdk", InstallMaxSdk));
#endif
        }

        private void InstallMaxSdk()
        {
            RegistryHelper.AddRegistryMax();
            InstallPackageHelper.Install(PackageConstant.MAX_PACKAGE_ID,
                () => { CreateAssets.CreateAsset<MaxSetting>(Constant.SETTINGS_FOLDER); });
        }
    }
}
