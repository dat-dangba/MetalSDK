using Metal;
using Metal.Ads;
using Metal.GameAnalytics;

public class AdsManager : BaseAdsManager
{
    protected override IAdsCondition Condition { get; } = new AdsCondition();

    protected override void OnAdRevenuePaid(AdRevenuePaidData data)
    {
#if HAS_METAL_GAME_ANALYTICS
        //todo: TrackAdRevenue
        Sdk.GameAnalytics.TrackAdRevenue(
            new AdRevenueEvent(
                data.Source,
                data.Revenue,
                data.AdRevenueNetwork,
                data.AdRevenueUnit,
                data.AdPlacement,
                data.Currency)
            {
                // LevelId = ,
                // LevelAttempt = ,
                // LevelProgress = ,
                // LevelType = "main"
            }
        );
#endif
    }
}
