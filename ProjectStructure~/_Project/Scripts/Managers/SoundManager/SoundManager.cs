using Metal;
using Metal.BaseGame;

public class SoundManager : BaseSoundManager<SoundId>
{
    protected override string SoundSoPath => "Assets/_Project/ScriptableObjects/SoundSo.asset";
    protected override SettingData SettingData => ServiceLocator.Game.Get<SaveManager>().DataSave.SettingData;
}
