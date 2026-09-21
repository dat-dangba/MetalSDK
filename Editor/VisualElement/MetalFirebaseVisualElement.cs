using System.Collections.Generic;
using Metal.Firebase;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Metal.Editor
{
    public class MetalFirebaseVisualElement : BaseVisualElement
    {
        public MetalFirebaseVisualElement()
        {
#if HAS_METAL_FIREBASE
            Add(new PackageInstalledVisualElement("Metal Firebase Sdk"));
            Add(new Label("Remote Config")
            {
                style =
                {
                    fontSize = 18,
                    marginTop = 20,
                    marginBottom = 5,
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            });
            ScriptableObject remoteConfigSetting = RemoteConfigSo.LoadConfig();
            DrawSetting(remoteConfigSetting);
            VisualElement scriptField = this.Q<PropertyField>("PropertyField:m_Script");
            if (scriptField != null)
            {
                scriptField.style.display = DisplayStyle.None;
            }
#else
            Add(new InstallPackageVisualElement("Metal Firebase Sdk", InstallMetalFirebase));
#endif
        }

        private void InstallMetalFirebase()
        {
            string token = MetalServicesEditor.GetToken();
            if (string.IsNullOrEmpty(token)) return;

            var packages = new List<string>();

            string metalFirebaseAppLink = $"https://{token}@github.com/dat-dangba/MetalFirebaseApp.git";
            if (!InstallPackageHelper.IsPackageInstalled(metalFirebaseAppLink))
            {
                packages.Add(metalFirebaseAppLink);
            }

            string metalFirebaseAnalyticsLink = $"https://{token}@github.com/dat-dangba/MetalFirebaseAnalytics.git";
            if (!InstallPackageHelper.IsPackageInstalled(metalFirebaseAnalyticsLink))
            {
                packages.Add(metalFirebaseAnalyticsLink);
            }

            string metalFirebaseCrashlyticsLink = $"https://{token}@github.com/dat-dangba/MetalFirebaseCrashlytics.git";
            if (!InstallPackageHelper.IsPackageInstalled(metalFirebaseCrashlyticsLink))
            {
                packages.Add(metalFirebaseCrashlyticsLink);
            }

            string metalFirebaseLink = $"https://{token}@github.com/dat-dangba/MetalFirebase.git";
            if (!InstallPackageHelper.IsPackageInstalled(metalFirebaseLink))
            {
                packages.Add(metalFirebaseLink);
            }

            InstallPackageHelper.Install(packages);
        }
    }
}
