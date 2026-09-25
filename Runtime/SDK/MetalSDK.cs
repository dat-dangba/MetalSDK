using UnityEngine;

namespace Metal
{
    public static class MetalSDK
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AfterSceneLoad()
        {
            GameObject sdk = new GameObject("MetalApplication");
            sdk.AddComponent<MetalApplication>();
        }

        public static bool IsEditor()
        {
#if UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }

        public static string GetToken()
        {
            string token = GeneralSetting.Load().SDKToken;
            if (!string.IsNullOrEmpty(token)) return token;
            MetalLog.Log("Chưa nhập token");
            return "";
        }
    }
}
