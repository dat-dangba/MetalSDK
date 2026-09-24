using System;
using Metal.Firebase;
using UnityEditor;
using UnityEngine;

namespace Metal.Editor
{
    public static class ScriptableObjectCreator
    {
        public static bool Create<T>(string assetPath) where T : ScriptableObject
        {
            var config = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (config != null) return false;

            EnsureFolderExists(assetPath);

            config = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(config, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            MetalLog.Log($"[ScriptableObjectCreator] Asset không tồn tại → đã tự tạo tại: {assetPath}");
            return true;
        }

        private static void EnsureFolderExists(string assetPath)
        {
            string folderPath = assetPath[..assetPath.LastIndexOf("/", StringComparison.Ordinal)];

            if (AssetDatabase.IsValidFolder(folderPath)) return;

            var parts = folderPath.Split('/');
            var currentPath = parts[0];

            for (int i = 1; i < parts.Length; i++)
            {
                var nextPath = currentPath + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(nextPath))
                {
                    AssetDatabase.CreateFolder(currentPath, parts[i]);
                }

                currentPath = nextPath;
            }
        }
    }
}
