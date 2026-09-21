using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using UnityEngine;

namespace Metal.Editor
{
// #if UNITY_IOS && UNITY_EDITOR
    public static class InfoPlistMerger
    {
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
        {
            if (buildTarget != BuildTarget.iOS)
                return;

            string plistPath = Path.Combine(pathToBuiltProject, "Info.plist");

            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            PlistElementDict root = plist.root;
            var configs = Resources.LoadAll<BaseBuildConfig>("");
            foreach (var cfg in configs)
            {
                ApplyIOS(root, cfg);
            }

            File.WriteAllText(plistPath, plist.WriteToString());
        }

        private static void ApplyIOS(PlistElementDict root, BaseBuildConfig config)
        {
            foreach (var field in config.GetBuildFields())
            {
                string key = field.Name;
                object rawValue = field.GetValue(config);

                if (rawValue == null) continue;

                switch (rawValue)
                {
                    case bool b:
                        root.SetBoolean(key, b);
                        break;
                    case int i:
                        root.SetInteger(key, i);
                        break;
                    case float f:
                        root.SetReal(key, f);
                        break;
                    case string s:
                        root.SetString(key == "admob_app_id" ? "GADApplicationIdentifier" : key, s);
                        break;
                    default:
                        root.SetString(key, rawValue.ToString());
                        break;
                }
            }
        }
    }
// #endif
}
