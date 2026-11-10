using System;
using System.Linq;
using System.Reflection;

[Serializable]
public class RemoteConfigData
{
    public bool AdjustEnabled = false;
    public string AppStoreId = "";

    #region AppOpenAd

    public bool AppOpenAdEnabled = false;

    public string AppOpenAdNetwork = "applovin";

    public AdConfig AppOpenAdStartGameConfig = new()
    {
        AdPlacement = AdPlacement.APP_OPEN_AD_START_GAME,
        Enabled = false,
        LevelShow = 0
    };

    public AdConfig AppOpenAdReturnGameConfig = new()
    {
        AdPlacement = AdPlacement.APP_OPEN_AD_RETURN_GAME,
        Enabled = false,
        LevelShow = 0
    };

    #endregion

    #region BannerAd

    public bool BannerAdEnabled = false;

    public string BannerAdNetwork = "applovin";

    public AdConfig BannerAdGamePlayConfig = new()
    {
        AdPlacement = AdPlacement.BANNER_AD_GAME_PLAY,
        Enabled = false,
        LevelShow = 0
    };

    public AdConfig BannerAdHomeConfig = new()
    {
        AdPlacement = AdPlacement.BANNER_AD_HOME,
        Enabled = false,
        LevelShow = 0
    };

    #endregion

    #region InterstitialAd

    public int InterstitialAdCoolDown = 60;

    public bool InterstitialAdEnabled = true;

    public string InterstitialAdNetwork = "applovin";

    #endregion

    #region RewardedAd

    public bool RewardedAdEnabled = true;

    public string RewardedAdNetwork = "applovin";

    #endregion

    #region AdConfig lookup

    private static readonly FieldInfo[] AdConfigFields = typeof(RemoteConfigData)
        .GetFields(BindingFlags.Public | BindingFlags.Instance)
        .Where(field => field.FieldType == typeof(AdConfig))
        .ToArray();

    public T GetAdConfig<T>(string adPlacement) where T : AdConfig
    {
        foreach (var field in AdConfigFields)
        {
            if (field.GetValue(this) is AdConfig config && config.AdPlacement == adPlacement)
            {
                return config as T;
            }
        }

        return null;
    }

    public bool TryGetAdConfig<T>(string adPlacement, out T adConfig) where T : AdConfig
    {
        adConfig = GetAdConfig<T>(adPlacement);
        return adConfig != null;
    }

    #endregion
}
