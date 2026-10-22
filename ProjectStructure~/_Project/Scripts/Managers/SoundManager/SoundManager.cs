using Metal;
using Metal.BaseGame;
#if UNITY_EDITOR
using Metal.Editor;
using UnityEditor;
#endif

public class SoundManager : BaseSoundManager<SoundId>
{
    protected override string SoundSoPath => "Assets/_Project/ScriptableObjects/SoundSo.asset";
    protected override SettingData SettingData => ServiceLocator.Game.Get<SaveManager>().DataSave.SettingData;

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    private static void CreateSoundSo()
    {
        ScriptableObjectCreator.Create<SoundSo>("Assets/_Project/ScriptableObjects/SoundSo.asset");
    }
#endif
}
