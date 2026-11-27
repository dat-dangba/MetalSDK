using UnityEditor;

namespace Metal.Editor
{
    [InitializeOnLoad]
    public static class SdkCreator
    {
        private const string MAX_SETTING_ASSET_PATH = "Assets/Resources/MetalSdk/Editor/MaxSetting.asset";
        private const string GENERAL_ASSET_PATH = "Assets/Resources/MetalSdk/Editor/GeneralSetting.asset";

        private const string MAX_MEDIATED_NETWORKS_ASSET_PATH =
            "Assets/Resources/MetalSdk/Editor/MaxMediatedNetworksSetting.asset";

        static SdkCreator()
        {
            // InitializeOnLoad có thể chạy trước khi import asset xong, đợi Editor khởi tạo xong mới tạo / load asset
            EditorApplication.delayCall += CreateAssets;
        }

        private static void CreateAssets()
        {
            ScriptableObjectCreator.Create<MaxSetting>(MAX_SETTING_ASSET_PATH);
            ScriptableObjectCreator.Create<GeneralSetting>(GENERAL_ASSET_PATH);
            bool isCreated = ScriptableObjectCreator.Create<MaxMediatedNetworks>(MAX_MEDIATED_NETWORKS_ASSET_PATH);
            if (!isCreated) return;

            var mediatedNetworks = AssetDatabase.LoadAssetAtPath<MaxMediatedNetworks>(MAX_MEDIATED_NETWORKS_ASSET_PATH);
            if (mediatedNetworks != null)
            {
                mediatedNetworks.CreateMediatedNetworks();
            }
        }
    }
}
