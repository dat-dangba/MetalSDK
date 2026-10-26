using Metal;
using Metal.BaseGame;

public abstract class BaseGameManager : BaseMonoBehaviour, IService
{
    private bool _isPlaying;

    public virtual void LevelStart()
    {
        _isPlaying = true;
        //todo: LevelStart
    }

    public virtual void LevelCompleted()
    {
        _isPlaying = false;
        //todo: LevelCompleted
    }

    public virtual void LevelFailed()
    {
        _isPlaying = false;
        //todo: LevelFailed
    }

    public virtual void BackToHome()
    {
        LevelAbandoned();
    }

    public virtual void Retry()
    {
        LevelAbandoned();
    }

    private void LevelAbandoned()
    {
        if (_isPlaying)
        {
            //todo: LevelAbandoned
            _isPlaying = false;
        }
    }

    public virtual void SoftFail()
    {
        //todo: LevelStep soft_fail
    }

    public virtual void Revive()
    {
        //todo: LevelStep revive
    }
}
