using UnityEngine;

public class LevelStateManager : MonoBehaviour
{
    public GameJobsCtrl jobsCtrl { get; private set; }
    public UIWindowsCtrl windowsCtrl { get; private set; }
    public LevelState State { get; private set; }

    private void OnEnable() 
    {
        windowsCtrl = FindObjectOfType<UIWindowsCtrl>();
        jobsCtrl = FindObjectOfType<GameJobsCtrl>();
    }

    private void Start() {
        SetState(new PreparationState(this));
    }

    private void Update() 
    {
        State.StateJob();
    }

    public void SetState(LevelState state)
    {
        State = state;
        // Debug.Log("Current level state now is " + state.ToString());
    }
}
