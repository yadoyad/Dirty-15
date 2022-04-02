public class ReadyState : LevelState
{
    public ReadyState(LevelStateManager levelStateManager) : base(levelStateManager)
    {
    }

    public override void StateJob()
    {
        if(jobsCtrl.jobCount > 0)
        {
            levelStateManager.SetState(new PreparationState(levelStateManager));
        }
        if(windowsCtrl.windowOpened)
        {
            levelStateManager.SetState(new PauseState(levelStateManager));
        }
    }
}
