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
#if !HAS_FIREBASE_APP
            Add(new InstallPackageVisualElement("Firebase App",
                () => { InstallPackageHelper.Install("https://github.com/dat-dangba/FirebaseApp.git"); }));
#elif !HAS_FIREBASE_ANALYTICS
            Add(new InstallPackageVisualElement("Firebase Analytics",
                () => { InstallPackageHelper.Install("https://github.com/dat-dangba/FirebaseAnalytics.git"); }));
#elif !HAS_FIREBASE_CRASHLYTICS
            Add(new InstallPackageVisualElement("Firebase Crashlytics",
                () => { InstallPackageHelper.Install("https://github.com/dat-dangba/FirebaseCrashlytics.git"); }));
#elif !HAS_FIREBASE_REMOTE_CONFIG
            Add(new InstallPackageVisualElement("Firebase Remote Config",
                () => { InstallPackageHelper.Install("https://github.com/dat-dangba/FirebaseRemoteConfig.git"); }));
#elif !HAS_FIREBASE_MESSAGING
            Add(new InstallPackageVisualElement("Firebase Messaging",
                () => { InstallPackageHelper.Install("https://github.com/dat-dangba/FirebaseMessaging.git"); }));
#elif !HAS_METAL_FIREBASE
            Add(new InstallPackageVisualElement("Metal Firebase Sdk",
                () =>
                {
                    string token = MetalServicesEditor.GetToken();
                    if (string.IsNullOrEmpty(token)) return;
                    string metalFirebaseLink = $"https://{token}@github.com/dat-dangba/MetalFirebase.git";
                    InstallPackageHelper.Install(metalFirebaseLink);
                }));
#else
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
            HideScript();
#endif
        }
    }
}
