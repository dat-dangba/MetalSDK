using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Metal.Editor
{
    public static class RegistryHelper
    {
        public static void AddRegistryMax()
        {
            var maxRegistry = new ScopedRegistry
            {
                Name = "AppLovin MAX Unity",
                URL = "https://unity.packages.applovin.com",
                Scopes = new List<string>
                {
                    "com.applovin.mediation.ads",
                    "com.applovin.mediation.adapters",
                    "com.applovin.mediation.dsp"
                }
            };
            AddRegistry(maxRegistry);
        }

        public static void AddRegistryGoogle()
        {
            var googleRegistry = new ScopedRegistry
            {
                Name = "google",
                URL = "https://package.openupm.com",
                Scopes = new List<string>
                {
                    "com.google"
                }
            };
            AddRegistry(googleRegistry);
        }

        public static void AddRegistryEdm4U()
        {
            var openUpm = new ScopedRegistry
            {
                Name = "package.openupm.com",
                URL = "https://package.openupm.com",
                Scopes = new List<string> { PackageConstant.EDM4_U_GOOGLE_PACKAGE_ID }
            };
            AddRegistry(openUpm);
        }

        private static void AddRegistry(ScopedRegistry scopedRegistry)
        {
            string manifestPath = Path.Combine(Application.dataPath, "..", "Packages", "manifest.json");
            if (!File.Exists(manifestPath))
            {
                MetalLog.Log($"Không tìm thấy file {manifestPath}");
                return;
            }

            string content = File.ReadAllText(manifestPath);

            UnityManifest manifest = JsonConvert.DeserializeObject<UnityManifest>(content);
            manifest.ScopedRegistries ??= new List<ScopedRegistry>();

            if (HasScope(manifest.ScopedRegistries, scopedRegistry.Name))
            {
                MetalLog.Log($"Scope {scopedRegistry.Name} đã được đăng ký");
                return;
            }

            manifest.ScopedRegistries.Add(scopedRegistry);

            string newJson = JsonConvert.SerializeObject(manifest, Formatting.Indented);
            File.WriteAllText(manifestPath, newJson);

            MetalLog.Log($"Đã thêm {scopedRegistry.Name} Registry thành công!");
            AssetDatabase.Refresh();
        }

        private static bool HasScope(List<ScopedRegistry> scopedRegistries, string name)
        {
            return scopedRegistries.Any(item => item.Name == name);
        }
    }
}
