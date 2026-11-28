using System.Collections.Generic;
using UnityEditor;

namespace Metal.Editor
{
    public static class BuildConfigLoader
    {
        /// <summary>
        /// Tất cả BaseBuildConfig trong project, dùng khi build (AndroidManifestMerger, InfoPlistMerger).
        /// Dùng AssetDatabase thay cho Resources.LoadAll: trong lúc build player, Resources.LoadAll không đọc
        /// được asset trong Editor/Resources (nơi lưu config để không bị đưa vào bản build).
        /// </summary>
        public static List<BaseBuildConfig> LoadAll()
        {
            var configs = new List<BaseBuildConfig>();
            foreach (string guid in AssetDatabase.FindAssets("t:" + nameof(BaseBuildConfig)))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var config = AssetDatabase.LoadAssetAtPath<BaseBuildConfig>(path);
                if (config != null)
                {
                    configs.Add(config);
                }
            }

            if (configs.Count == 0)
            {
                MetalLog.LogWarning("[BuildConfigLoader] Không tìm thấy BaseBuildConfig nào trong project", true);
            }

            return configs;
        }
    }
}
