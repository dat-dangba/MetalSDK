using System.IO;
using UnityEditor;
using UnityEngine;

namespace Metal.Editor
{
    [InitializeOnLoad]
    public static class CreateAssets
    {
        static CreateAssets()
        {
#if HAS_MAX
            CreateAsset<MAXSetting>(Constant.SettingsFolder);
#endif
            CreateAsset<GeneralSetting>(Constant.SETTINGS_FOLDER);
        }

        public static T CreateAsset<T>(string folderPath) where T : ScriptableObject
        {
            string path = $"Assets/Resources/{folderPath}";

            EnsureFolderExists(path);

            string configName = typeof(T).Name;
            string fullPath = Path.Combine(path, $"{configName}.asset");
            if (File.Exists(fullPath)) return null;
            ScriptableObject asset = ScriptableObject.CreateInstance(typeof(T));
            AssetDatabase.CreateAsset(asset, fullPath);
            AssetDatabase.SaveAssets();
            MetalLog.Log($"Create {configName}: {fullPath}");
            return asset as T;
        }

        private static void EnsureFolderExists(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath)) return;

            if (Directory.Exists(folderPath)) return;

            Directory.CreateDirectory(folderPath);
        }
    }
}
