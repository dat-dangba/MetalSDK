using Metal;
using Metal.GameAnalytics;
using Metal.Firebase;

public static class Sdk
{
    private static FirebaseManager FirebaseManager => ServiceLocator.Sdk.Get<FirebaseManager>();
    public static RemoteConfigData RemoteConfig => FirebaseManager.RemoteConfig.Data;
    public static Analytics FirebaseAnalytics => FirebaseManager.Analytics;

    public static MetalGameAnalytics GameAnalytics => ServiceLocator.Sdk.Get<MetalGameAnalytics>();
    public static AdsManager Ads => ServiceLocator.Sdk.Get<AdsManager>();
    public static IAPManager IAP => ServiceLocator.Sdk.Get<IAPManager>();
}
