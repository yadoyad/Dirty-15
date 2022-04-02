using UnityEngine;
public class InputManager : MonoBehaviour
{
    public FigureCtrl figureCtrl { get; private set; }
    public AbilitiesCtrl abilitiesCtrl { get; private set; }
    public MatchFinder matchFinder { get; private set; }
    public FigureScriptedAnimations fAnim { get; private set; }
    public InputState State { get; private set; }
    public LevelStateManager lsm { get; private set; }

    private void OnEnable() 
    {
        figureCtrl = FindObjectOfType<FigureCtrl>();
        fAnim = FindObjectOfType<FigureScriptedAnimations>();
        matchFinder = FindObjectOfType<MatchFinder>();
        lsm = FindObjectOfType<LevelStateManager>();
        abilitiesCtrl = FindObjectOfType<AbilitiesCtrl>();
    }
    private void Start() 
    {
        SetState(new FirstTouchState(this));
    }
    private void Update() 
    {
        State.DetectStuff();
    }

    public void SetState(InputState state)
    {
        State = state;
        // Debug.Log("Current input state now is " + state.ToString());
    }
}
