#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEditor.Android;
using UnityEngine;

namespace Metal.Editor
{
    public class AndroidManifestMerger : IPostGenerateGradleAndroidProject
    {
        private const string ANDROID_NS = "http://schemas.android.com/apk/res/android";
        private const string TOOLS_NS = "http://schemas.android.com/tools";
        private const string ADMOB_APP_ID_KEY = "admob_app_id";
        private const string ADMOB_APP_ID_META_NAME = "com.google.android.gms.ads.APPLICATION_ID";

        public int callbackOrder { get; }

        public void OnPostGenerateGradleAndroidProject(string pathToBuiltProject)
        {
            var launcherManifestPath = GetLauncherManifestPath(pathToBuiltProject);
            var launcherPath = GetLauncherPath(pathToBuiltProject);

            SetAllowBackupAndIsGame(launcherManifestPath);

            var configDict = GetAllConfig();
            bool success = MergeBuildGradle(launcherPath, configDict);
            if (!success) return;
            MergeManifest(launcherManifestPath, configDict);
        }

        private void SetAllowBackupAndIsGame(string manifestPath)
        {
            var xml = new XmlDocument();
            xml.Load(manifestPath);
            var manifest = xml.SelectSingleNode("/manifest");
            if (manifest?.SelectSingleNode("application") is XmlElement application)
            {
                application.SetAttribute("allowBackup", ANDROID_NS, "false");
                application.SetAttribute("isGame", ANDROID_NS, "true");
            }

            xml.Save(manifestPath);
        }

        private string GetLauncherManifestPath(string pathToBuiltProject)
        {
            string manifestPath;
            if (pathToBuiltProject.Contains("unityLibrary"))
            {
                var pathProject = pathToBuiltProject.Replace("unityLibrary", "");
                manifestPath = Path.Combine(pathProject, "launcher/src/main/AndroidManifest.xml");
            }
            else if (pathToBuiltProject.Contains("launcher"))
            {
                manifestPath = Path.Combine(pathToBuiltProject, "src/main/AndroidManifest.xml");
            }
            else
            {
                manifestPath = Path.Combine(pathToBuiltProject, "launcher/src/main/AndroidManifest.xml");
            }

            return manifestPath;
        }

        private string GetLauncherPath(string pathToBuiltProject)
        {
            string launcherPath;

            if (pathToBuiltProject.Contains("unityLibrary"))
            {
                var pathProject = pathToBuiltProject.Replace("unityLibrary", "");
                launcherPath = pathProject + "/launcher";
            }
            else if (pathToBuiltProject.Contains("launcher"))
            {
                launcherPath = pathToBuiltProject;
            }
            else
            {
                launcherPath = pathToBuiltProject + "/launcher";
            }

            return launcherPath;
        }

        private Dictionary<string, string> GetAllConfig()
        {
            var configs = BuildConfigLoader.LoadAll();
            Dictionary<string, string> configDict = new();

            foreach (var config in configs)
            {
                foreach (var field in config.GetBuildFields())
                {
                    string key = field.Name;
                    string value = ConvertValue(field.GetValue(config));

                    if (string.IsNullOrEmpty(value)) continue;

                    if (configDict.ContainsKey(key))
                    {
                        Debug.LogWarning(
                            $"[AndroidManifestMerger] Key '{key}' bị trùng, dùng giá trị của {config.GetType().Name}");
                    }

                    configDict[key] = value;
                }
            }

            return configDict;
        }

        private void MergeManifest(string manifestPath, Dictionary<string, string> configDict)
        {
            if (!File.Exists(manifestPath))
            {
                Debug.LogWarning($"Không tìm thấy file manifest: {manifestPath}");
                return;
            }

            var xml = new XmlDocument();
            xml.Load(manifestPath);
            var manifest = xml.SelectSingleNode("/manifest");
            if (manifest == null)
            {
                Debug.LogWarning($"Không tìm thấy thẻ manifest trong file {manifestPath}");
                return;
            }

            var application = manifest.SelectSingleNode("application");
            if (application == null)
            {
                Debug.LogWarning($"Không tìm thấy thẻ application trong manifest {manifestPath}");
                return;
            }

            foreach (string key in configDict.Keys)
            {
                // Tên thẻ meta-data trong manifest (admob_app_id phải dùng tên chuẩn của Google Mobile Ads)
                string metaName = key == ADMOB_APP_ID_KEY ? ADMOB_APP_ID_META_NAME : key;
                // Giá trị luôn là placeholder, Gradle thay bằng manifestPlaceholders trong build.gradle
                string metaValue = $"${{{key}}}";

                XmlElement meta = FindMetaData(application, metaName);
                if (meta == null)
                {
                    meta = xml.CreateElement("meta-data");
                    meta.SetAttribute("name", ANDROID_NS, metaName);
                    application.AppendChild(meta);
                }

                meta.SetAttribute("value", ANDROID_NS, metaValue);

                if (metaName == ADMOB_APP_ID_META_NAME)
                {
                    // Plugin Google Mobile Ads cũng ghi APPLICATION_ID (lấy từ GoogleMobileAdsSettings) vào manifest
                    // của unityLibrary: luôn dùng giá trị placeholder của launcher, tránh lỗi xung đột khi merge
                    EnsureToolsNamespace(xml.DocumentElement);
                    meta.SetAttribute("replace", TOOLS_NS, "android:value");
                }
            }

            xml.Save(manifestPath);
        }

        private static void EnsureToolsNamespace(XmlElement manifest)
        {
            if (!manifest.HasAttribute("xmlns:tools"))
            {
                manifest.SetAttribute("xmlns:tools", TOOLS_NS);
            }
        }

        private static XmlElement FindMetaData(XmlNode application, string metaName)
        {
            foreach (XmlNode node in application.ChildNodes)
            {
                if (node is XmlElement element
                    && element.Name == "meta-data"
                    && element.GetAttribute("name", ANDROID_NS) == metaName)
                {
                    return element;
                }
            }

            return null;
        }

        private bool MergeBuildGradle(string launcherPath, Dictionary<string, string> configDict)
        {
            if (configDict.Count == 0)
            {
                Debug.LogWarning($"Không có config cần thêm");
                return false;
            }

            string buildGradlePath = Path.Combine(launcherPath, "build.gradle");

            if (!File.Exists(buildGradlePath))
            {
                Debug.LogWarning($"Không tìm thấy file buildGradle: {buildGradlePath}");
                return false;
            }

            string content = File.ReadAllText(buildGradlePath);
            var placeHolders = GetPlaceHolders(configDict);

            if (content.Contains("defaultConfig"))
            {
                int startOfDefaultConfig = content.IndexOf("defaultConfig", StringComparison.Ordinal);
                if (startOfDefaultConfig == -1)
                {
                    Debug.LogWarning("Không thể chèn manifestPlaceholders vào defaultConfig trong build.gradle");
                    return false;
                }

                int firstOpenBrace = content.IndexOf('{', startOfDefaultConfig);
                int lastCloseBrace = FindClosingBrace(content, firstOpenBrace);
                if (lastCloseBrace == -1)
                {
                    Debug.LogWarning("Không thể chèn manifestPlaceholders vào defaultConfig trong build.gradle");
                    return false;
                }

                content = content.Insert(lastCloseBrace, placeHolders);
                File.WriteAllText(buildGradlePath, content);
                Debug.Log("Đã chèn manifestPlaceholders vào defaultConfig thành công!");
                return true;
            }

            Debug.LogWarning("Không tìm thấy thẻ defaultConfig trong build.gradle");
            return false;
        }

        private int FindClosingBrace(string text, int openBraceIndex)
        {
            int count = 0;
            for (int i = openBraceIndex; i < text.Length; i++)
            {
                if (text[i] == '{') count++;
                else if (text[i] == '}')
                {
                    count--;
                    if (count == 0) return i;
                }
            }

            return -1;
        }

        private string GetPlaceHolders(Dictionary<string, string> configDict)
        {
            var lines = configDict
                .Select(x => $"\t\t\t{x.Key}: \"{x.Value}\"");
            string placeHolders = "\n\t\tmanifestPlaceholders = [\n" +
                                  string.Join(",\n", lines) +
                                  "\n\t\t]\n\t";
            return placeHolders;
        }

        private string ConvertValue(object value)
        {
            if (value == null) return null;

            return value switch
            {
                bool b => b ? "true" : "false",
                int i => i.ToString(),
                float f => f.ToString(System.Globalization.CultureInfo.InvariantCulture),
                string s => s,
                _ => value.ToString()
            };
        }
    }
}
#endif
