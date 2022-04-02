using UnityEngine;
using System;

public class MovesCtrl : MonoBehaviour
{
    public static MovesCtrl instance;

    public int totalMoves {get; private set;} = 0;
    public int movesCount {get; private set;} = 0;
    private GameObject counterObject;
    private UIMovesCounter movesCounter;
    public event Action OnCountChange;

    private void Awake() 
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable() 
    {
        movesCounter = FindObjectOfType<UIMovesCounter>();
        counterObject = movesCounter.GetMyObj();
    }

    private void Start() 
    {
        FigureScriptedAnimations.instance.OnMovementStart += CountMoves;
        totalMoves = LevelLoadManager.instance.currentLevel.movesRequired;
        if(totalMoves != 0)
        {
            counterObject.SetActive(true);
            UpdateMovesText();
        }
        else
            counterObject.SetActive(false);
    }

    private void OnDisable() 
    {
        FigureScriptedAnimations.instance.OnMovementStart -= CountMoves;
    }

    private void CountMoves()
    {
        movesCount++;
        OnCountChange?.Invoke();
        if(totalMoves != 0)
        {
            UpdateMovesText();
            if(movesCount <= totalMoves)
            {
                // Debug.Log("Still got moves");
            }
            else
            {
                if(!FinishCtrl.instance.finishReached)
                    UIWindowsCtrl.instance.ShowLevelFailedWindow();
            }
        }
    }

    public void UpdateMovesText()
    {
        movesCounter.UpdateCounterText();
    }
}
