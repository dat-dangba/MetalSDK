using System.Collections.Generic;
using Metal;
using Metal.BaseGame;

public class CurrencyManager : GameResourceManager<ItemType>
{
    protected override Dictionary<ItemType, SaveValue<int>> Amounts
    {
        get
        {
            SaveManager saveManager = ServiceLocator.Game.Get<SaveManager>();
            return new Dictionary<ItemType, SaveValue<int>>()
            {
                { ItemType.Coin, saveManager.DataSave.CoinSaveValue }
            };
        }
    }
}
