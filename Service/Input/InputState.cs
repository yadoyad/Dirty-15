public abstract class InputState
{
    protected InputManager _inputManager;

    public InputState(InputManager inputManager)
    {
        _inputManager = inputManager;
    }

    public abstract void DetectStuff();
}
