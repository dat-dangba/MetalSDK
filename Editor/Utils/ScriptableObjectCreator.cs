using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Metal.Editor
{
    public static class ScriptableObjectCreator
    {
        /// <returns>true nếu vừa tạo asset mới, false nếu asset đã tồn tại</returns>
        public static bool Create<T>(string assetPath) where T : ScriptableObject
        {
            var config = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (config != null) return false;

            // File đã có trên đĩa nhưng load ra null (VD chưa import xong khi mở project / rebuild Library):
            // không được tạo lại, nếu không sẽ ghi đè dữ liệu đã có bằng giá trị mặc định
            if (File.Exists(assetPath))
            {
                MetalLog.LogWarning(
                    $"[ScriptableObjectCreator] {assetPath} đã tồn tại nhưng chưa load được, bỏ qua không tạo lại");
                return false;
            }

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
