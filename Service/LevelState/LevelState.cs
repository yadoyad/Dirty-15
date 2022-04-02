public abstract class LevelState
{
    protected LevelStateManager levelStateManager;
    protected UIWindowsCtrl windowsCtrl;
    protected GameJobsCtrl jobsCtrl;
    public LevelState(LevelStateManager levelStateManager)
    {
        this.levelStateManager = levelStateManager;
        windowsCtrl = levelStateManager.windowsCtrl;
        jobsCtrl = levelStateManager.jobsCtrl;
    }
    public abstract void StateJob();
}
