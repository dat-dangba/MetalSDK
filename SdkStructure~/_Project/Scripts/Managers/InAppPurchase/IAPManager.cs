using Metal.GameAnalytics;
using Metal.IAP;

public class IAPManager : BaseIAPManager
{
    protected override byte[] TangleData => null;

    protected override void OnPurchaseProductCompeted(bool success, PurchaseData purchaseData)
    {
#if HAS_METAL_GAME_ANALYTICS
        if (success)
        {
            //todo: TrackInAppPurchase
            Sdk.GameAnalytics.TrackInAppPurchase(
                new InAppPurchaseEvent(
                    (double)purchaseData.LocalizedPrice,
                    purchaseData.IsoCurrencyCode,
                    purchaseData.Token
                )
                {
                    // LevelId = ,
                    // LevelAttempt = ,
                    // LevelProgress = ,
                    // LevelType = ,
                    // IapPlacement = ,
                    // IapProductId = ,
                    // IapOrderId = ,
                    // IapTransactionId =
                }
            );
        }
#endif
    }
}
