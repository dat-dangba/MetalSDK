using Metal.Ads;

public class AdsCondition : IAdsCondition
{
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
        return false;
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
        return false;
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
        return false;
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
        return true;
    }
}
