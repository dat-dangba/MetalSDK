using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Metal.Editor
{
    public static class ScriptableObjectCreator
    {
        /// <param name="assetPath">Đường dẫn asset</param>
        /// <param name="legacyAssetPath">
        /// Đường dẫn cũ (bản SDK trước). Có asset ở đây thì chuyển sang assetPath (giữ GUID) thay vì tạo mới,
        /// để không mất dữ liệu đã nhập.
        /// </param>
        /// <returns>true nếu vừa tạo asset mới, false nếu asset đã tồn tại (hoặc vừa được chuyển từ đường dẫn cũ)</returns>
        public static bool Create<T>(string assetPath, string legacyAssetPath = null) where T : ScriptableObject
        {
            MoveLegacyAsset(assetPath, legacyAssetPath);

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

        private static void MoveLegacyAsset(string assetPath, string legacyAssetPath)
        {
            if (string.IsNullOrEmpty(legacyAssetPath) || !File.Exists(legacyAssetPath)) return;

            if (File.Exists(assetPath))
            {
                MetalLog.LogWarning(
                    $"[ScriptableObjectCreator] Có cả {legacyAssetPath} và {assetPath}, đang dùng {assetPath}. " +
                    $"Kiểm tra lại rồi xoá {legacyAssetPath} (asset trong Resources vẫn bị đưa vào bản build)", true);
                return;
            }

            EnsureFolderExists(assetPath);

            string error = AssetDatabase.MoveAsset(legacyAssetPath, assetPath);
            if (!string.IsNullOrEmpty(error))
            {
                MetalLog.LogError($"[ScriptableObjectCreator] Không chuyển được {legacyAssetPath} → {assetPath}: {error}", true);
                return;
            }

            MetalLog.LogWarning($"[ScriptableObjectCreator] Đã chuyển {legacyAssetPath} → {assetPath}", true);
            DeleteFolderIfEmpty(legacyAssetPath[..legacyAssetPath.LastIndexOf("/", StringComparison.Ordinal)]);
        }

        private static void DeleteFolderIfEmpty(string folderPath)
        {
            if (!AssetDatabase.IsValidFolder(folderPath)) return;

            bool isEmpty = Directory.GetFileSystemEntries(folderPath).All(entry => entry.EndsWith(".DS_Store"));
            if (isEmpty)
            {
                AssetDatabase.DeleteAsset(folderPath);
            }
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
