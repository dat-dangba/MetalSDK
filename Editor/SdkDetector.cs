using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Compilation;

namespace Metal.Editor
{
    [InitializeOnLoad]
    public class SdkDetector
    {
        private static readonly Dictionary<string, string> SdkDefinitions = new()
        {
            { "HAS_MAX", PackageConstant.MAX_PACKAGE_ID },
            { "HAS_AUTO_REFERENCE", PackageConstant.AUTO_REFERENCE_PACKAGE_ID },
            { "HAS_ADJUST", PackageConstant.ADJUST_PACKAGE_ID },
            { "HAS_IN_APP_PURCHASING", PackageConstant.IAP_PACKAGE_ID },
            { "HAS_METAL_BASE_GAME", PackageConstant.METAL_BASE_GAME_PACKAGE_ID },
            { "HAS_METAL_ANALYTICS", PackageConstant.METAL_ANALYTICS_PACKAGE_ID },
            { "HAS_METAL_ADS", PackageConstant.METAL_ADS_PACKAGE_ID },
            { "HAS_METAL_IAP", PackageConstant.METAL_IAP_PACKAGE_ID },
            { "HAS_METAL_FIREBASE", PackageConstant.METAL_FIREBASE_PACKAGE_ID },
            { "HAS_FIREBASE_APP", PackageConstant.FIREBASE_APP_PACKAGE_ID },
            { "HAS_FIREBASE_ANALYTICS", PackageConstant.FIREBASE_ANALYTICS_PACKAGE_ID },
            { "HAS_FIREBASE_CRASHLYTICS", PackageConstant.FIREBASE_CRASHLYTICS_PACKAGE_ID },
            { "HAS_FIREBASE_REMOTE_CONFIG", PackageConstant.FIREBASE_REMOTE_CONFIG_PACKAGE_ID },
            { "HAS_FIREBASE_MESSAGING", PackageConstant.FIREBASE_MESSAGING_PACKAGE_ID },
        };

        static SdkDetector()
        {
            if (IsInPackage())
            {
                CompilationPipeline.compilationFinished += _ => UpdateDefineSymbolsAndReload();
            }
            else
            {
                BuildTargetGroup currentGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
                DefineSymbolIfNeeded(currentGroup, "HAS_AUTO_REFERENCE", true);
                DefineSymbolIfNeeded(currentGroup, "HAS_METAL_BASE_GAME", true);
                DefineSymbolIfNeeded(currentGroup, "HAS_METAL_FIREBASE", true);
                DefineSymbolIfNeeded(currentGroup, "HAS_FIREBASE_APP", true);
                DefineSymbolIfNeeded(currentGroup, "HAS_FIREBASE_ANALYTICS", true);
                DefineSymbolIfNeeded(currentGroup, "HAS_FIREBASE_CRASHLYTICS", true);
                DefineSymbolIfNeeded(currentGroup, "HAS_FIREBASE_REMOTE_CONFIG", true);
                DefineSymbolIfNeeded(currentGroup, "HAS_FIREBASE_MESSAGING", true);
                DefineSymbolIfNeeded(currentGroup, "HAS_IN_APP_PURCHASING", true);
                DefineSymbolIfNeeded(currentGroup, "HAS_METAL_IAP", true);
            }
        }

        private static void UpdateDefineSymbolsAndReload()
        {
            BuildTargetGroup currentGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            foreach (var item in SdkDefinitions)
            {
                bool isInstalled = IsInstalled(item.Value);
                if (!DefineSymbolIfNeeded(currentGroup, item.Key, isInstalled)) continue;
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private static bool DefineSymbolIfNeeded(BuildTargetGroup targetGroup, string defineSymbol, bool isInstalled)
        {
            NamedBuildTarget namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(targetGroup);
            PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget, out string[] currentDefineSymbols);
            if (!currentDefineSymbols.Contains(defineSymbol) && isInstalled)
            {
                PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget,
                    currentDefineSymbols.Append(defineSymbol).ToArray());
                return true;
            }

            if (currentDefineSymbols.Contains(defineSymbol) && !isInstalled)
            {
                PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget,
                    currentDefineSymbols.Except(new[] { defineSymbol }).ToArray());
                return true;
            }

            return false;
        }

        private static bool IsInstalled(string packageId)
        {
            return InstallPackageHelper.IsPackageInstalled(packageId);
        }

        private static bool IsInPackage()
        {
            return GetFilePath().Contains("Package");
        }

        private static string GetFilePath([CallerFilePath] string path = "")
        {
            return path;
        }
    }
}
