using Metal;
using Metal.BaseGame;

public class VibrateManager : BaseVibrateManager
{
    protected override SettingData SettingData => ServiceLocator.Game.Get<SaveManager>().DataSave.SettingData;
}
