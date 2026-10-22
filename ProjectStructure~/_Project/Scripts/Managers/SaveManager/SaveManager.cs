using Metal.BaseGame;
using UnityEngine;

public class SaveManager : BaseSaveManager<DataSave>
{
    private const int VERSION = 0;

    protected override void Migrate(int fromVersion)
    {
    }

    protected override int Version()
    {
        return VERSION;
    }

    [ContextMenu("Save Data")]
    public override void SaveData()
    {
        base.SaveData();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }
}
