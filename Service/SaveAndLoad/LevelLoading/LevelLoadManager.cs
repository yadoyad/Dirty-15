using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class LevelLoadManager : MonoBehaviour
{
    public static LevelLoadManager instance;
    public UISceneTransitionWindow loadingScreen;
    public LevelDescription currentLevel {get; private set;}
    public LevelDescription levelToLoad {get; private set;}

    [Header("Описания уровней")]
    public List<LevelDescription> levels = new List<LevelDescription>();
    private void Awake() 
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(this);
    }
    private void OnEnable() 
    {
        SetCurrentLevel();
    }

    private void SetCurrentLevel()
    {
        currentLevel = levels.Find(x => x.levelBuildIndex == SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevel(LevelDescription level)
    {
        SceneManager.LoadScene(level.levelBuildIndex);
    }

    public void LoadLevelFromMenu(LevelDescription level)
    {
        SetCurrentLevel();
        SetLevelToLoad("LLFM", level);
        loadingScreen = FindObjectOfType<UISceneTransitionWindow>();
        loadingScreen.StartTransition();
        StartCoroutine(WaitABit());
    }

    public void LoadMenu()
    {
        SetCurrentLevel();
        var nextLevel = levels.Find(x => x.levelBuildIndex == 0);
        SetLevelToLoad("LoadMenu", nextLevel);
        loadingScreen = FindObjectOfType<UISceneTransitionWindow>();
        loadingScreen.StartTransition();
        StartCoroutine(WaitABit());
    }

    public void LoadNextLevel()
    {
        SetCurrentLevel();
        if(!levelToLoad || levelToLoad.levelBuildIndex == currentLevel.levelBuildIndex)
        {
            SetLevelToLoad("LoadNextLevel", FindNextLevel());
        }

        loadingScreen = FindObjectOfType<UISceneTransitionWindow>();
        loadingScreen.StartTransition();
        StartCoroutine(WaitABit());
    }

    public void ReloadCurrentLevel()
    {
        SetCurrentLevel();
        SetLevelToLoad("ReloadCurrentLevel", currentLevel);
        loadingScreen = FindObjectOfType<UISceneTransitionWindow>();
        loadingScreen.StartTransition();
        StartCoroutine(WaitABit());
    }

    //Было создано для дебага, но у меня так подгорело, что лень назад переделывать
    private void SetLevelToLoad(string s, LevelDescription level)
    {
        levelToLoad = level;
    }

    //Вызывается напрямую в UISceneTransition
    public void ContinueTransition()
    {
        LoadLevel(levelToLoad);
        SceneManager.sceneLoaded += EndTransition;
    }

    public void EndTransition(Scene scene, LoadSceneMode sceneMode)
    {
        loadingScreen.EndTransition();
        SetCurrentLevel();
        SetLevelToLoad("EndTransition", FindNextLevel());
        SceneManager.sceneLoaded -= EndTransition;
    }

    private LevelDescription FindNextLevel()
    {
        LevelDescription nextLevel = levels.Find(x => x.levelBuildIndex == SceneManager.GetActiveScene().buildIndex + 1);
        if(!nextLevel)
        {
            nextLevel = levels.Find(x => x.levelBuildIndex == 0);
        }
        return nextLevel;
    }

    IEnumerator WaitABit()
    {
        yield return new WaitForSeconds(.5f);
        ContinueTransition();
    }
}
