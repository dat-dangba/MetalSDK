using System.Collections.Generic;
using Metal;
using UnityEngine;

public class RewardManager : BaseRewardManager<ItemType>
{
    private CurrencyManager _currency;
    private BoosterManager _booster;
    private LivesManager _lives;

    public override void Initialize()
    {
        _currency = ServiceLocator.Game.Get<CurrencyManager>();
        _booster = ServiceLocator.Game.Get<BoosterManager>();
        _lives = ServiceLocator.Game.Get<LivesManager>();
#if UNITY_EDITOR
        Debug.LogWarning($"[RewardManager] Initialized");
#endif
    }

    public override void Receive(ItemType type, int amount)
    {
        switch (type)
        {
            case ItemType.Coin:
                _currency.Add(type, amount);
                break;
            case ItemType.UnlimitedLives:
                _lives.AddUnlimitedTime(amount);
                break;
            // case ItemType.Hammer:
            // case ItemType.Magnet:
            //     _booster.Add(type, amount);
            //     break;
        }
    }

    public void Receive(RewardData rewardData)
    {
        Receive(rewardData.Type, rewardData.Amount);
    }

    public void Receive(List<RewardData> rewardDatas)
    {
        foreach (var item in rewardDatas)
        {
            Receive(item);
        }
    }
}
