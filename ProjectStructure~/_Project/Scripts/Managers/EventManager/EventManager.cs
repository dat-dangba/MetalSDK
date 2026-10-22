using Metal;
using Metal.BaseGame;

public class EventManager : BaseEventManager
{
    protected override INetworkTime NetworkTime => ServiceLocator.Game.Get<NetworkTimeManager>();
}
