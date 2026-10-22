using System.Collections.Generic;
using Metal;
using Metal.BaseGame;
using UnityEngine;

public class RewardManager : BaseMonoBehaviour, IInitializable
{
    private CurrencyManager _currency;
    private BoosterManager _booster;
    private LivesManager _lives;

    public void Initialize()
    {
        _currency = ServiceLocator.Game.Get<CurrencyManager>();
        _booster = ServiceLocator.Game.Get<BoosterManager>();
        _lives = ServiceLocator.Game.Get<LivesManager>();
#if UNITY_EDITOR
        Debug.LogWarning($"[RewardManager] Initialized");
#endif
    }

    public void Reward(ItemType type, int amount)
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

    public void Reward(RewardData rewardData)
    {
        Reward(rewardData.Type, rewardData.Amount);
    }

    public void Reward(List<RewardData> rewardDatas)
    {
        foreach (var item in rewardDatas)
        {
            Reward(item);
        }
    }
}
