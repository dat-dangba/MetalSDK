namespace Metal
{
    using UnityEngine;

    public static class MetalLog
    {
        [HideInCallstack]
        public static void Log(string message, bool forceLog = false)
        {
            if (ShouldLog() || forceLog)
            {
                Debug.Log($"MetalSDK - {message}");
            }
        }

        [HideInCallstack]
        public static void LogWarning(string message, bool forceLog = false)
        {
            if (ShouldLog() || forceLog)
            {
                Debug.LogWarning($"MetalSDK - {message}");
            }
        }

        [HideInCallstack]
        public static void LogError(string message, bool forceLog = false)
        {
            if (ShouldLog() || forceLog)
            {
                Debug.LogError($"MetalSDK - {message}");
            }
        }

        private static bool ShouldLog()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return true;
#else
            return false;
#endif
        }
    }
}
