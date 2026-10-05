using UnityEditor;
using UnityEngine.UIElements;
#if HAS_METAL_ADS
using Metal.Ads;
#endif
#if HAS_MAX
using AppLovinMax.Scripts.IntegrationManager.Editor;
#endif

namespace Metal.Editor
{
    public class MaxVisualElement : BaseVisualElement
    {
        public MaxVisualElement()
        {
#if !HAS_MAX
            Add(new InstallPackageVisualElement("MAX", InstallMaxSdk));
#else
            Add(new PackageInstalledVisualElement("MAX"));
            Add(new VisualElement
            {
                style =
                {
                    marginTop = 10
                }
            });
            DrawSetting(MaxSetting.Load());
            Button installMediatedNetworksButton = new Button(InstallMediatedNetworks)
            {
                text = "Install Mediated Networks"
            };
            Add(installMediatedNetworksButton);

            Button setupMaxSdkButton = new Button(SetupMaxSdk)
            {
                text = "Setup MAX"
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
            MetalAdsBuildConfig metalAdsBuildConfig = MetalAdsBuildConfig.Load();
            metalAdsBuildConfig.admob_app_id = maxSetting.GoogleAdmobAppId;
            EditorUtility.SetDirty(metalAdsBuildConfig);
#endif

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
