public class PreparationState : LevelState
{
    bool stateEnter = false;
    public PreparationState(LevelStateManager levelStateManager) : base(levelStateManager)
    {
    }

    public override void StateJob()
    {
        AfterStart();
        if(!jobsCtrl.isBusy)
        {
            if(jobsCtrl.jobCount > 0)
            {
                levelStateManager.SetState(new PreparationState(levelStateManager));
            }
            else
            {
                if(AnimatorsHub.instance.allReady)
                    levelStateManager.SetState(new ReadyState(levelStateManager));
            }
        }
        if(windowsCtrl.windowOpened)
        {
            levelStateManager.SetState(new PauseState(levelStateManager));
        }
    }

    private void AfterStart()
    {
        if(!stateEnter)
        {
            if(jobsCtrl.jobCount > 0)
            {
                jobsCtrl.LetPreparationsBegin();
            }
            stateEnter = true;
        }
    }
}
