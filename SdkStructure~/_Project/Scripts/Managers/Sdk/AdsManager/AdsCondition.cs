using Metal;
using Metal.Ads;

public class AdsCondition : IAdsCondition
{
    private int Level => ServiceLocator.Game.Get<SaveManager>().DataSave.Level + 1;

    public int GetInterstitialAdCooldown()
    {
        return Sdk.RemoteConfig.InterstitialAdCoolDown;
    }

    public bool IsNoAds()
    {
        return false;
    }

    public string BannerAdNetwork()
    {
        return Sdk.RemoteConfig.BannerAdNetwork;
    }

    public bool IsBannerAdEnabled()
    {
        return Sdk.RemoteConfig.BannerAdEnabled;
    }

    public bool IsBannerAdEnabledAt(string adPlacement)
    {
        return IsAdEnabledAt(adPlacement, Level);
    }

    public string AppOpenAdNetwork()
    {
        return Sdk.RemoteConfig.AppOpenAdNetwork;
    }

    public bool IsAppOpenAdEnabled()
    {
        return Sdk.RemoteConfig.AppOpenAdEnabled;
    }

    public bool IsAppOpenAdEnabledAt(string adPlacement)
    {
        return IsAdEnabledAt(adPlacement, Level);
    }

    public string InterstitialAdNetwork()
    {
        return Sdk.RemoteConfig.InterstitialAdNetwork;
    }

    public bool IsInterstitialAdEnabled()
    {
        return Sdk.RemoteConfig.InterstitialAdEnabled;
    }

    public bool IsInterstitialAdEnabledAt(string adPlacement)
    {
        switch (adPlacement)
        {
            case AdPlacement.INTERSTITIAL_AD_CLAIM_COIN_VICTORY:
                bool result = Sdk.RemoteConfig.TryGetAdConfig(adPlacement, out WinCountAdConfig adConfig);
                return result && adConfig.CanShow(Level - 1);
            default:
                return IsAdEnabledAt(adPlacement, Level);
        }
    }

    public string RewardedAdNetwork()
    {
        return Sdk.RemoteConfig.RewardedAdNetwork;
    }

    public bool IsRewardedAdEnabled()
    {
        return Sdk.RemoteConfig.RewardedAdEnabled;
    }

    public bool IsRewardedAdEnabledAt(string adPlacement)
    {
        int lv = adPlacement == AdPlacement.REWARDED_AD_CLAIM_COIN_VICTORY ? Level - 1 : Level;
        return IsAdEnabledAt(adPlacement, lv);
    }

    private bool IsAdEnabledAt(string adPlacement, int level)
    {
        if (Sdk.RemoteConfig == null) return false;
        bool result = Sdk.RemoteConfig.TryGetAdConfig(adPlacement, out AdConfig adConfig);
        return result && adConfig.Enabled && level >= adConfig.LevelShow;
    }
}
