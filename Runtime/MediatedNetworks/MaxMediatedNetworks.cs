using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Metal
{
    [System.Serializable]
    public struct MediatedNetwork
    {
        public string Name;
        public string Package;
        public string AndroidVersion;
        public string IOSVersion;

        public MediatedNetwork(string name, string package, string androidVersion, string iosVersion)
        {
            Name = name;
            Package = package;
            AndroidVersion = androidVersion;
            IOSVersion = iosVersion;
        }
    }

    public class MaxMediatedNetworks : ScriptableObject
    {
        private const string RESOURCE_PATH = "MetalSdk/Editor/MaxMediatedNetworksSetting";

        public static MaxMediatedNetworks Load()
        {
            return Resources.Load<MaxMediatedNetworks>(RESOURCE_PATH);
        }

        public List<MediatedNetwork> MediatedNetworks = new();

        public List<string> GetAllPackages()
        {
            var packages = new List<string>();
            foreach (var item in MediatedNetworks)
            {
                packages.Add($"com.applovin.mediation.adapters.{item.Package}.android@{item.AndroidVersion}");
                packages.Add($"com.applovin.mediation.adapters.{item.Package}.ios@{item.IOSVersion}");
            }

            return packages;
        }

        [ContextMenu("Create Mediated Networks")]
        public void CreateMediatedNetworks()
        {
#if UNITY_EDITOR
            MediatedNetworks = new List<MediatedNetwork>
            {
                new("BidMachine", "bidmachine", "3080001.0.0", "308020000.0.0"),
                new("Chartboost", "chartboost", "9140101.0.0", "9140200.0.0"),
                new("DT Exchange", "fyber", "8050100.0.0", "8050100.0.0"),
                new("Facebook", "facebook", "6220001.0.0", "6220100.0.0"),
                new("Google", "google", "25050000.0.0", "13120000.0.0"),
                new("InMobi", "inmobi", "11050100.0.0", "11050000.0.0"),
                new("ironSource", "ironsource", "907000000.0.0", "907000000.0.0"),
                new("Liftoff", "vungle", "7080000.0.0", "7080000.0.0"),
                new("Mintegral", "mintegral", "17022100.0.0", "801090000.0.0"),
                new("Pangle", "bytedance", "804000500.0.0", "803000900.0.0"),
                new("Unity Ads", "unityads", "4210001.0.0", "4210001.0.0"),
                new("Verve", "verve", "3090300.0.0", "3090300.0.0"),
                new("Yandex", "yandex", "8060000.0.0", "8060000.0.0"),
            };
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }
    }
}
