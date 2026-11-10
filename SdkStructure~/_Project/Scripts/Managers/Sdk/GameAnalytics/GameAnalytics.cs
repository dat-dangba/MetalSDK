using Metal.GameAnalytics;

public class GameAnalytics : BaseGameAnalytics
{
    protected override bool IsAdjustEnabled => Sdk.RemoteConfig.AdjustEnabled;
}
