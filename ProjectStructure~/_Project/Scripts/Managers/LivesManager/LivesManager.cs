using Metal;
using Metal.BaseGame;

public class LivesManager : BaseLivesManager
{
    private const float REGEN_SECONDS_PER_LIFE = 1800;

    protected override float RegenSecondsPerLife => REGEN_SECONDS_PER_LIFE;
    protected override LivesData LivesData => ServiceLocator.Game.Get<SaveManager>().DataSave.LivesData;
    protected override INetworkTime NetworkTime => ServiceLocator.Game.Get<NetworkTimeManager>();
}
