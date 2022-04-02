public class PauseState : LevelState
{
    public PauseState(LevelStateManager levelStateManager) : base(levelStateManager)
    {
    }

    public override void StateJob()
    {
        if(!windowsCtrl.windowOpened)
        {
            levelStateManager.SetState(new PreparationState(levelStateManager));
        }
    }
}
