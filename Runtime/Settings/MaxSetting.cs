using UnityEngine;

namespace Metal
{
    public class MaxSetting : ScriptableObject
    {
        public static MaxSetting Load()
        {
            return Resources.Load<MaxSetting>("MetalSdk/Editor/MaxSetting");
        }

        public string MaxSdkKey =
            "8tzHmVzO2RTSGw_Zn_JbB25mkbZpSCg6peEkO9bxE8mCP1rT_QQNT6d79XmTM9qmq74HgLLsfoMvs_xlu1KQ16";

        public bool ConsentFlowEnabled = true;
        public string ConsentFlowPrivacyPolicyUrl = "https://www.google.com";
        public string GoogleAdmobAppId = "ca-app-pub-3940256099942544~3347511713";
#if HAS_MAX
        public MaxSdkBase.ConsentFlowUserGeography DebugUserGeography = MaxSdkBase.ConsentFlowUserGeography.Gdpr;
#endif
    }
}
