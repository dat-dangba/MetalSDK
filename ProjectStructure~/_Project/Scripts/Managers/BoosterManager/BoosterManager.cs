using System.Collections.Generic;
using Metal;
using Metal.BaseGame;
using Teo.AutoReference;
using UnityEngine;

public class BoosterManager : BaseBoosterManager<ItemType, BoosterSo>
{
    [SerializeField, GetInChildren, IgnoreSelf]
    private List<MonoBehaviour> _boosterHandlers;

    protected override List<MonoBehaviour> BoosterHandler => _boosterHandlers;

    protected override Dictionary<ItemType, SaveValue<int>> Amounts
    {
        get
        {
            SaveManager saveManager = ServiceLocator.Game.Get<SaveManager>();
            return new Dictionary<ItemType, SaveValue<int>>()
            {
                // { ItemType.Hammer, saveManager.DataSave.HammerSaveValue },
            };
        }
    }
}
