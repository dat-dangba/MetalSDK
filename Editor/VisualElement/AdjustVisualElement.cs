#if HAS_ADJUST
using System.Threading.Tasks;
using UnityEditor;
using AdjustSdk;
#endif

namespace Metal.Editor
{
    public class AdjustVisualElement : BaseVisualElement
    {
        public AdjustVisualElement()
        {
#if !HAS_ADJUST
            Add(new InstallPackageVisualElement("Adjust", InstallAdjust));
#else
            DrawAdjust();
            SetupAdjustIOS();
#endif
        }

        private void DrawAdjust()
        {
#if HAS_ADJUST
            Add(new PackageInstalledVisualElement("Adjust"));
            DrawSetting(AdjustSettings.Instance);
#endif
        }

        private async void SetupAdjustIOS()
        {
#if HAS_ADJUST
            if (AssetDatabase.LoadAssetAtPath<AdjustSettings>(AdjustSettings.AdjustSettingsExportPath)) return;

            AdjustSettings adjustSettings = AdjustSettings.Instance;
            await Task.Delay(100);
            AdjustSettings.iOSFrameworkAdSupport = true;
            AdjustSettings.iOSFrameworkAdServices = true;
            AdjustSettings.iOSFrameworkAppTrackingTransparency = true;
            AdjustSettings.iOSFrameworkStoreKit = true;

            EditorUtility.SetDirty(AdjustSettings.Instance);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Clear();
            DrawAdjust();
#endif
        }

        private void InstallAdjust()
        {
            InstallPackageHelper.Install(PackageConstant.ADJUST_LINK_INSTALL);
        }
    }
}
