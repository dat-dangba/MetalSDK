namespace Metal
{
    using System.Diagnostics;
    using UnityEngine;
    using Debug = UnityEngine.Debug;

    /// <summary>
    /// Log của SDK.
    /// - Log(message) / LogWarning(message) / LogError(message): chỉ có ở Editor và Development Build.
    ///   Bản release compiler xoá hẳn lời gọi, nên chuỗi nội suy / JsonConvert trong tham số cũng không được tính.
    /// - Overload có forceLog: luôn được compile, forceLog = true thì log cả ở bản release (dùng cho lỗi quan trọng).
    /// </summary>
    public static class MetalLog
    {
        private const string EDITOR = "UNITY_EDITOR";
        private const string DEVELOPMENT_BUILD = "DEVELOPMENT_BUILD";

        [HideInCallstack, Conditional(EDITOR), Conditional(DEVELOPMENT_BUILD)]
        public static void Log(string message)
        {
            Debug.Log($"MetalSDK - {message}");
        }

        [HideInCallstack]
        public static void Log(string message, bool forceLog)
        {
            if (ShouldLog() || forceLog)
            {
                Debug.Log($"MetalSDK - {message}");
            }
        }

        [HideInCallstack, Conditional(EDITOR), Conditional(DEVELOPMENT_BUILD)]
        public static void LogWarning(string message)
        {
            Debug.LogWarning($"MetalSDK - {message}");
        }

        [HideInCallstack]
        public static void LogWarning(string message, bool forceLog)
        {
            if (ShouldLog() || forceLog)
            {
                Debug.LogWarning($"MetalSDK - {message}");
            }
        }

        [HideInCallstack, Conditional(EDITOR), Conditional(DEVELOPMENT_BUILD)]
        public static void LogError(string message)
        {
            Debug.LogError($"MetalSDK - {message}");
        }

        [HideInCallstack]
        public static void LogError(string message, bool forceLog)
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
