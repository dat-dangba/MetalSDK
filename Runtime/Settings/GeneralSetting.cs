using UnityEngine;

namespace Metal
{
    public class GeneralSetting : ScriptableObject
    {
        public static GeneralSetting Load()
        {
            return Resources.Load<GeneralSetting>("MetalSdk/Editor/GeneralSetting");
        }

        public string SDKToken = "";
    }
}
