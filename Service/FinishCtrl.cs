using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishCtrl : MonoBehaviour
{
    public static FinishCtrl instance;
    public bool finishReached {get; private set;} = false;

    private void Awake() 
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void LevelFinished()
    {
        finishReached = true;
        if(!UIWindowsCtrl.instance.windowOpened)
            UIWindowsCtrl.instance.ShowLevelFinishedWindow();
    }
}
