using UnityEngine;

namespace Metal
{
    public class GeneralSetting : ScriptableObject
    {
        public static GeneralSetting Load()
        {
            return Resources.Load<GeneralSetting>("MetalSdk/GeneralSetting");
        }

        public string SDKToken = "";
    }
}
