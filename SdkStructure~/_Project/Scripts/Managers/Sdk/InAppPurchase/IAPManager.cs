using Metal;
using Metal.GameAnalytics;
using Metal.IAP;
using UnityEngine;

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

    /// <summary>
    /// Cộng vật phẩm cho giao dịch đã thanh toán nhưng chưa nhận (app bị kill khi đang mua, Ask to Buy...).
    /// BaseIAPManager tự gọi sau MetalApplication.OnGameLoadingCompleted (save đã load).
    /// Lưu ý: purchaseData.Id là id trên store (iOS có tiền tố Application.identifier + ".").
    /// </summary>
    /// <returns>true khi đã cộng vật phẩm (order được confirm), false để giữ order cho lần mở app sau</returns>
    protected override bool GrantUnprocessedPurchase(PurchaseData purchaseData)
    {
        ProductSo product = GetProductData<ProductSo>(purchaseData.Id);
        if (product == null)
        {
            Debug.LogError($"[IAPManager] Không tìm thấy ProductSo cho {purchaseData.Id} trong InAppPurchase.asset");
            return false;
        }

        // Gói IsRemoveAds là NonConsumable: trạng thái đã mua do BaseIAPManager quản lý (IsProductPurchased)
        if (product.RewardDatas is { Count: > 0 })
        {
            ServiceLocator.Game.Get<RewardManager>().Receive(product.RewardDatas);
        }

        return true;
    }
}
