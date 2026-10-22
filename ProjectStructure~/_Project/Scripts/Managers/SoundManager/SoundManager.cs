using Metal;
using Metal.BaseGame;

public class SoundManager : BaseSoundManager<SoundId>
{
    protected override string SoundSoPath => "";
    protected override SettingData SettingData => ServiceLocator.Game.Get<SaveManager>().DataSave.SettingData;
}
