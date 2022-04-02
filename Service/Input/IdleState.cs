public class IdleState : InputState
{
    bool stateEnter = false;
    FigureScriptedAnimations fAnim;
    MatchFinder mFinder;
    LevelStateManager lsm;
    public IdleState(InputManager inputManager) : base(inputManager)
    {
    }

    public override void DetectStuff()
    {
        AfterStart();
        if(lsm.State.GetType() == typeof(ReadyState))
        {
            _inputManager.SetState(new FirstTouchState(_inputManager));
        }
    }

    private void AfterStart()
    {
        if(!stateEnter)
        {
            stateEnter = true;
            fAnim = _inputManager.fAnim;
            mFinder = _inputManager.matchFinder;
            lsm = _inputManager.lsm;
        }
    }
}
