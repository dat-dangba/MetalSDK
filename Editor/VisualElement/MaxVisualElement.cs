using AppLovinMax.Scripts.IntegrationManager.Editor;
using UnityEditor;
using UnityEngine.UIElements;

namespace Metal.Editor
{
    public class MaxVisualElement : BaseVisualElement
    {
        public MaxVisualElement()
        {
#if !HAS_MAX
            Add(new InstallPackageVisualElement("MAX Sdk", InstallMaxSdk));
#else
            DrawSetting(MaxSetting.Load());
            Button installMediatedNetworksButton = new Button(InstallMediatedNetworks)
            {
                text = "Install Mediated Networks"
            };
            Add(installMediatedNetworksButton);

            Button setupMaxSdkButton = new Button(SetupMaxSdk)
            {
                text = "Setup MAX Sdk"
            };
            Add(setupMaxSdkButton);
#endif
        }

        private void InstallMaxSdk()
        {
            RegistryHelper.AddRegistryMax();
            InstallPackageHelper.Install(PackageConstant.MAX_PACKAGE_ID);
        }

        private void InstallMediatedNetworks()
        {
#if HAS_MAX
            MaxMediatedNetworks mediatedNetworks = MaxMediatedNetworks.Load();
            InstallPackageHelper.Install(mediatedNetworks.GetAllPackages());
#endif
        }

        private void SetupMaxSdk()
        {
#if HAS_MAX
            MaxSetting maxSetting = MaxSetting.Load();

            AppLovinSettings.Instance.SdkKey = maxSetting.MaxSdkKey;
            AppLovinSettings.Instance.AdMobAndroidAppId = maxSetting.GoogleAdmobAppId;
            AppLovinSettings.Instance.AdMobIosAppId = maxSetting.GoogleAdmobAppId;
            AppLovinSettings.Instance.SaveAsync();

            AppLovinInternalSettings.Instance.ConsentFlowEnabled = maxSetting.ConsentFlowEnabled;
            AppLovinInternalSettings.Instance.ConsentFlowPrivacyPolicyUrl = maxSetting.ConsentFlowPrivacyPolicyUrl;
            AppLovinInternalSettings.Instance.DebugUserGeography = maxSetting.DebugUserGeography;
            AppLovinInternalSettings.Instance.Save();

            EditorUtility.SetDirty(AppLovinInternalSettings.Instance);
            EditorUtility.SetDirty(AppLovinSettings.Instance);
#endif

#if HAS_METAL_ADS
            MetalAdsBuildConfig metalAdsBuildConfig = MetalServices.GetBuildConfig<MetalAdsBuildConfig>();
            metalAdsBuildConfig.admob_app_id = serializedObject.FindProperty("GoogleAdmobAppId").stringValue;
            EditorUtility.SetDirty(metalAdsBuildConfig);
#endif

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
