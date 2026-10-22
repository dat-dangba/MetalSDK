using Metal;
using Metal.BaseGame;

public class ReconnectButton : BaseButton
{
    protected override void OnClick()
    {
        ServiceLocator.Game.Get<InternetManager>().Retry();
    }
}
