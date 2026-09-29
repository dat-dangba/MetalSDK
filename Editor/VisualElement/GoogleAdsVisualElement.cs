using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Metal.Editor
{
    public class GoogleAdsVisualElement : BaseVisualElement
    {
        private const string GOOGLE_APP_ID = "ca-app-pub-3940256099942544~3347511713";

        public GoogleAdsVisualElement()
        {
#if !HAS_GOOGLE_ADS
            Add(new InstallPackageVisualElement("Google Ads", InstallGoogleAds));
#else
            AddSetupGoogleAds();
#endif
        }

        private void AddSetupGoogleAds()
        {
            Add(new PackageInstalledVisualElement("Google Ads"));

            ScriptableObject scriptableObject = Resources.Load<ScriptableObject>("GoogleMobileAdsSettings");

            if (!scriptableObject)
            {
                Add(new Button(SetupGoogleAds)
                {
                    text = "Setup Google Ads",
                    style =
                    {
                        marginTop = 10
                    }
                });
            }
            else
            {
                Add(new VisualElement
                {
                    style =
                    {
                        marginTop = 10
                    }
                });
                DrawSetting(scriptableObject);
            }
        }

        private void InstallGoogleAds()
        {
            RegistryHelper.AddRegistryGoogle();
            InstallPackageHelper.Install(PackageConstant.GOOGLE_ADS_PACKAGE_ID, () => { });
        }

        private void SetupGoogleAds()
        {
            SetIds();
            Clear();
            AddSetupGoogleAds();
        }

        private void SetIds()
        {
            var asset = FindSettings();
            if (asset == null)
            {
                // Menu này gọi LoadInstance() nên sẽ tạo asset nếu chưa có
                EditorApplication.ExecuteMenuItem("Assets/Google Mobile Ads/Settings...");
                asset = FindSettings();
            }

            if (asset == null)
            {
                Debug.LogError("Không tìm thấy GoogleMobileAdsSettings.");
                return;
            }

            var so = new SerializedObject(asset);
            so.FindProperty("adMobAndroidAppId").stringValue = GOOGLE_APP_ID;
            so.FindProperty("adMobIOSAppId").stringValue = GOOGLE_APP_ID;
            so.ApplyModifiedProperties();

            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        static Object FindSettings()
        {
            var guids = AssetDatabase.FindAssets("t:GoogleMobileAdsSettings");
            return guids.Length == 0
                ? null
                : AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }
    }
}
