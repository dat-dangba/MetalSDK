using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

#if HAS_METAL_FIREBASE
using Metal.Firebase;
#endif

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

#if !HAS_FIREBASE_APP
            string firebaseAppLink = $"https://github.com/dat-dangba/FirebaseApp.git";
            if (!InstallPackageHelper.IsPackageInstalled(firebaseAppLink))
            {
                packages.Add(firebaseAppLink);
            }
#endif

#if !HAS_FIREBASE_ANALYTICS
            string firebaseAnalyticsLink = $"https://github.com/dat-dangba/FirebaseAnalytics.git";
            if (!InstallPackageHelper.IsPackageInstalled(firebaseAnalyticsLink))
            {
                packages.Add(firebaseAnalyticsLink);
            }
#endif

#if !HAS_FIREBASE_CRASHLYTICS
            string firebaseCrashlyticsLink = $"https://github.com/dat-dangba/FirebaseCrashlytics.git";
            if (!InstallPackageHelper.IsPackageInstalled(firebaseCrashlyticsLink))
            {
                packages.Add(firebaseCrashlyticsLink);
            }
#endif

#if !HAS_FIREBASE_REMOTE_CONFIG
            string firebaseRemoteConfigLink = $"https://github.com/dat-dangba/FirebaseRemoteConfig.git";
            if (!InstallPackageHelper.IsPackageInstalled(firebaseRemoteConfigLink))
            {
                packages.Add(firebaseRemoteConfigLink);
            }
#endif

#if !HAS_FIREBASE_MESSAGING
            string firebaseMessagingLink = $"https://github.com/dat-dangba/FirebaseMessaging.git";
            if (!InstallPackageHelper.IsPackageInstalled(firebaseMessagingLink))
            {
                packages.Add(firebaseMessagingLink);
            }
#endif

            string metalFirebaseLink = $"https://{token}@github.com/dat-dangba/MetalFirebase.git";
            if (!InstallPackageHelper.IsPackageInstalled(metalFirebaseLink))
            {
                packages.Add(metalFirebaseLink);
            }

            InstallPackageHelper.Install(packages);
        }
    }
}
