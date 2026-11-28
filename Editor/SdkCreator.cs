using UnityEditor;

namespace Metal.Editor
{
    [InitializeOnLoad]
    public static class SdkCreator
    {
        // Editor/Resources: Resources.Load đọc được trong Editor nhưng không bị đưa vào bản build
        private const string FOLDER = "Assets/Editor/Resources/MetalSdk/";

        // Vị trí ở bản SDK cũ (Resources/.../Editor vẫn bị đưa vào bản build), tự chuyển sang FOLDER
        private const string LEGACY_FOLDER = "Assets/Resources/MetalSdk/Editor/";

        private const string MAX_SETTING_FILE = "MaxSetting.asset";
        private const string GENERAL_FILE = "GeneralSetting.asset";
        private const string MAX_MEDIATED_NETWORKS_FILE = "MaxMediatedNetworksSetting.asset";

        static SdkCreator()
        {
            // InitializeOnLoad có thể chạy trước khi import asset xong, đợi Editor khởi tạo xong mới tạo / load asset
            EditorApplication.delayCall += CreateAssets;
        }

        private static void CreateAssets()
        {
            ScriptableObjectCreator.Create<MaxSetting>(FOLDER + MAX_SETTING_FILE, LEGACY_FOLDER + MAX_SETTING_FILE);
            ScriptableObjectCreator.Create<GeneralSetting>(FOLDER + GENERAL_FILE, LEGACY_FOLDER + GENERAL_FILE);
            bool isCreated = ScriptableObjectCreator.Create<MaxMediatedNetworks>(
                FOLDER + MAX_MEDIATED_NETWORKS_FILE, LEGACY_FOLDER + MAX_MEDIATED_NETWORKS_FILE);
            if (!isCreated) return;

            var mediatedNetworks =
                AssetDatabase.LoadAssetAtPath<MaxMediatedNetworks>(FOLDER + MAX_MEDIATED_NETWORKS_FILE);
            if (mediatedNetworks != null)
            {
                mediatedNetworks.CreateMediatedNetworks();
            }
        }
    }
}
