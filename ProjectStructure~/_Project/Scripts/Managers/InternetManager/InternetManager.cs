using Metal.BaseGame;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InternetManager : BaseInternetManager
{
    protected override string NoInternetPrefabPath => "Assets/_Project/Prefabs/NoInternet/NoInternetUI.prefab";

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    private static void AddScript()
    {
        GameObject prefab =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Project/Prefabs/NoInternet/NoInternetUI.prefab");
        if (prefab == null) return;
        bool results = prefab.TryGetComponent(typeof(NoInternetUI), out var noInternetUI);
        if (!results)
        {
            prefab.AddComponent<NoInternetUI>();
        }

        Transform retryButton = prefab.transform.GetChild(0).Find("RetryButton");
        if (retryButton == null) return;

        bool hasRetryButton = retryButton.TryGetComponent(typeof(ReconnectButton), out var reconnectButton);
        if (!hasRetryButton)
        {
            retryButton.gameObject.AddComponent<ReconnectButton>();
        }
    }
#endif
}
