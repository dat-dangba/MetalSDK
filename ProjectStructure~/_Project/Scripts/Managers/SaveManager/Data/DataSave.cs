using System;
using Metal.BaseGame;
using UnityEngine;

[Serializable]
public partial class DataSave : BaseDataSave
{
    [SerializeField] private SaveValue<int> _coin = new(0);
    [SerializeField] private SaveValue<int> _level = new(0);
    [SerializeField] private SaveValue<LivesData> _livesData = new(new LivesData());
    [SerializeField] private SaveValue<SettingData> _settingData = new(new SettingData());
}
