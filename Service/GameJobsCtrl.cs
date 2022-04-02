using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class GameJobsCtrl : MonoBehaviour
{
    public static GameJobsCtrl instance;
    public bool isBusy {get; private set;} = false;
    public int jobCount {get; private set;} = 0;
    public event Action preps;

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

    public void LetPreparationsBegin()
    {
        StartCoroutine(PrepRoutine());
    }

    public void AddJob(Action act)
    {
        preps -= act;
        preps += act;
        jobCount++;
    }

    //Рутина запускает все методы, добавленные в preps по очереди и удаляет при этом
    IEnumerator PrepRoutine()
    {
        isBusy = true;

        List<Action> invocationList = new List<Action>();
        try{
            invocationList = preps.GetInvocationList().OfType<Action>().ToList();
        }catch (Exception ex){
            Debug.Log(ex.Message);
        }

        if(invocationList.Count > 0)
        {
            foreach(Action act in invocationList)
            {
                act.Invoke();
                preps -= act;
                jobCount--;
                yield return null;
            }
        }
        
        //Временная проверка
        if(jobCount != 0)
        {
            Debug.Log("jobCount is not zero, its " + jobCount);
            // jobCount = 0;
        }
        isBusy = false;
    }
}
