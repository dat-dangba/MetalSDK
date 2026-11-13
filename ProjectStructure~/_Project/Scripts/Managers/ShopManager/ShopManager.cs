using System;
using Metal;
using Metal.BaseGame;
using UnityEngine;

public class ShopManager : BaseShopManager<ItemType>
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

    public override void PurchaseProduct(string productId, string iapPlacement, Action<string> onPurchaseSuccess,
        Action<string> onPurchaseFailed)
    {
    }

    public override bool TrySpend(ItemType currencyType, int price)
    {
        return _currency.Spend(currencyType, price);
    }

    public bool TrySpendCoin(int price)
    {
        return TrySpend(ItemType.Coin, price);
    }

    public bool TryBuyBooster(ItemType boosterType)
    {
        if (!_booster.Validate(boosterType)) return false;

        BoosterSo data = _booster.GetBoosterData(boosterType);

        if (!TrySpendCoin(data.PurchasePrice)) return false;

        _reward.Receive(boosterType, data.PurchaseAmount);
        return true;
    }
}
