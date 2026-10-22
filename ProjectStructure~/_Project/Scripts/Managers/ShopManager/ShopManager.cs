using System;
using Metal;
using Metal.BaseGame;
using UnityEngine;

public class ShopManager : BaseShopManager<BoosterSo, ProductSo>
{
    private BoosterManager _booster;
    private CurrencyManager _currency;
    private RewardManager _reward;

    public override void Initialize()
    {
        _booster = ServiceLocator.Game.Get<BoosterManager>();
        _currency = ServiceLocator.Game.Get<CurrencyManager>();
        _reward = ServiceLocator.Game.Get<RewardManager>();
#if UNITY_EDITOR
        Debug.LogWarning($"[ShopManager] Initialized");
#endif
    }

    public override void PurchaseBooster(BoosterSo boosterSo,
        Action<BoosterSo> onPurchaseSuccess,
        Action<BoosterSo> onPurchaseFailed)
    {
        if (!_booster.Validate(boosterSo.Type)) return;

        BoosterSo data = _booster.GetBoosterData(boosterSo.Type);

        if (!_currency.Spend(ItemType.Coin, data.PurchasePrice))
        {
            onPurchaseFailed?.Invoke(boosterSo);
            return;
        }

        _reward.Reward(boosterSo.Type, data.PurchaseAmount);
        onPurchaseSuccess?.Invoke(boosterSo);
    }

    public override void PurchaseProduct(ProductSo productSo, Action<ProductSo> onPurchaseSuccess,
        Action<ProductSo> onPurchaseFailed)
    {
    }
}
