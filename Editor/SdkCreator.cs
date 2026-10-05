using System.IO;
using System.Threading.Tasks;
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

        private const string FOLDER_PATH = "Assets/Resources/MetalSdk/Editor";

        static SdkCreator()
        {
            EnsureFolderExists();
            CreateAssets();
        }

        private static async void CreateAssets()
        {
            await Task.Delay(100);
            ScriptableObjectCreator.Create<MaxSetting>(MAX_SETTING_ASSET_PATH);
            ScriptableObjectCreator.Create<GeneralSetting>(GENERAL_ASSET_PATH);
            bool result = ScriptableObjectCreator.Create<MaxMediatedNetworks>(MAX_MEDIATED_NETWORKS_ASSET_PATH);
            if (result)
            {
                LoadMaxMediatedNetworks();
            }
        }

        private static async void LoadMaxMediatedNetworks()
        {
            await Task.Delay(100);
            MaxMediatedNetworks.Load().CreateMediatedNetworks();
        }

        private static void EnsureFolderExists()
        {
            if (Directory.Exists(FOLDER_PATH)) return;

            Directory.CreateDirectory(FOLDER_PATH);
            AssetDatabase.Refresh();
        }
    }
}
