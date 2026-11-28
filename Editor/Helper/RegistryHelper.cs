using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        /// <summary>
        /// Thêm scoped registry vào Packages/manifest.json. Chỉ sửa mục scopedRegistries, giữ nguyên mọi field khác
        /// (testables, enableLockFile, resolutionStrategy...). Scope đã có ở bất kỳ registry nào thì bỏ qua,
        /// registry cùng URL đã có thì gộp scope vào đó (Package Manager báo lỗi nếu 1 scope nằm ở 2 registry).
        /// </summary>
        private static void AddRegistry(ScopedRegistry scopedRegistry)
        {
            string manifestPath = Path.Combine(Application.dataPath, "..", "Packages", "manifest.json");
            if (!File.Exists(manifestPath))
            {
                MetalLog.Log($"Không tìm thấy file {manifestPath}");
                return;
            }

            JObject manifest;
            try
            {
                manifest = JObject.Parse(File.ReadAllText(manifestPath));
            }
            catch (Exception ex)
            {
                MetalLog.LogError($"Không đọc được {manifestPath}: {ex.Message}", true);
                return;
            }

            if (manifest["scopedRegistries"] is not JArray registries)
            {
                registries = new JArray();
                manifest["scopedRegistries"] = registries;
            }

            HashSet<string> existingScopes = registries.OfType<JObject>()
                .SelectMany(GetScopes)
                .ToHashSet();
            List<string> missingScopes = scopedRegistry.Scopes.Where(scope => !existingScopes.Contains(scope)).ToList();

            if (missingScopes.Count == 0)
            {
                MetalLog.Log($"Scope của {scopedRegistry.Name} đã được đăng ký");
                return;
            }

            JObject sameUrlRegistry = registries.OfType<JObject>()
                .FirstOrDefault(registry => IsSameUrl((string)registry["url"], scopedRegistry.URL));

            if (sameUrlRegistry != null)
            {
                if (sameUrlRegistry["scopes"] is not JArray scopes)
                {
                    scopes = new JArray();
                    sameUrlRegistry["scopes"] = scopes;
                }

                foreach (string scope in missingScopes)
                {
                    scopes.Add(scope);
                }
            }
            else
            {
                registries.Add(new JObject
                {
                    ["name"] = scopedRegistry.Name,
                    ["url"] = scopedRegistry.URL,
                    ["scopes"] = new JArray(missingScopes)
                });
            }

            File.WriteAllText(manifestPath, manifest.ToString(Formatting.Indented));

            MetalLog.Log($"Đã thêm {scopedRegistry.Name} Registry thành công!");
            AssetDatabase.Refresh();
        }

        private static IEnumerable<string> GetScopes(JObject registry)
        {
            return registry["scopes"] is JArray scopes ? scopes.Values<string>() : Enumerable.Empty<string>();
        }

        private static bool IsSameUrl(string a, string b)
        {
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return false;
            return string.Equals(a.TrimEnd('/'), b.TrimEnd('/'), StringComparison.OrdinalIgnoreCase);
        }
    }
}
