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

        static SdkCreator()
        {
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
    }
}
