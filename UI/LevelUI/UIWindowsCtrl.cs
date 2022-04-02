using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIWindowsCtrl : MonoBehaviour
{
    public static UIWindowsCtrl instance;
    public GameObject overlay;
    public UIWindow pauseWindow;
    public UIWindow levelFinishedWindow;
    public UIWindow levelFailedWindow;
    public UIWindow levelInstructionsWindow;
    public float instructionsAnimationTime = 1f;
    [HideInInspector] public bool windowOpened {get; private set;}

    //Затемнение bg
    [Header("Затемнение бэкграунда")]
    public Image background;
    [SerializeField] private float darkBGAlpha = 60f;
    [SerializeField] private float bgFadeAnimationTime = .4f;

    private IEnumerator currentRoutine;

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
        SceneManager.sceneLoaded += ShowInstructions;
    }

    public void ShowPauseWindow()
    {
        overlay.SetActive(false);
        DarkenBG();
        OpenWindow(pauseWindow);
        windowOpened = true;
    }
    public void ClosePauseWindow()
    {
        CloseWindow(pauseWindow);
        BrightenBG();
        overlay.SetActive(true);
        windowOpened = false;
    }

    public void ShowInstructions(Scene scene, LoadSceneMode sceneMode)
    {
        overlay.SetActive(false);
        DarkenBG();
        OpenWindow(levelInstructionsWindow);
        windowOpened = true;
        SceneManager.sceneLoaded -= ShowInstructions;
        currentRoutine = CloseWindowWithDelay(levelInstructionsWindow, instructionsAnimationTime);
        StartCoroutine(currentRoutine);
    }

    public void CloseInstructions()
    {
        StopCoroutine(currentRoutine);
        CloseWindow(levelInstructionsWindow);
        BrightenBG();
        overlay.SetActive(true);
        windowOpened = false;
    }

    public void ShowLevelFinishedWindow()
    {
        overlay.SetActive(false);
        DarkenBG();
        UILevelFinishedPopUp.instance.SetUpTextFields();
        OpenWindow(levelFinishedWindow);
        windowOpened = true;
    }

    public void ShowLevelFailedWindow()
    {
        overlay.SetActive(false);
        DarkenBG();
        OpenWindow(levelFailedWindow);
        windowOpened = true;
    }

    public void StartLevelTransition()
    {
        // CloseWindow(levelFinishedWindow);
        BrightenBG();
        windowOpened = true;
        overlay.SetActive(false);
        LevelLoadManager.instance.LoadNextLevel();
    }

    public void RestartLevel()
    {
        BrightenBG();
        windowOpened = true;
        overlay.SetActive(false);
        LevelLoadManager.instance.ReloadCurrentLevel();
    }

    public void LoadMenu()
    {
        BrightenBG();
        windowOpened = true;
        overlay.SetActive(false);
        LevelLoadManager.instance.LoadMenu();
    }

    private void OpenWindow(UIWindow window)
    {
        window.OpenWindow();
    }
    private void CloseWindow(UIWindow window)
    {
        window.CloseWindow();
    }

    private void DarkenBG()
    {
        StartCoroutine(ChangeBGAlpha(darkBGAlpha, true));
    }

    private void BrightenBG()
    {
        StartCoroutine(ChangeBGAlpha(0f, false));
    }
    IEnumerator ChangeBGAlpha(float alpha, bool open)
    {   
        float timeElapsed = 0f;

        while(timeElapsed < bgFadeAnimationTime)
        {
            timeElapsed += Time.deltaTime;
            Color tempColor = background.color;
            float currentAlpha = Mathf.Lerp(tempColor.a, alpha, timeElapsed / bgFadeAnimationTime);
            tempColor.a = alpha > 0 ? currentAlpha / 255f : currentAlpha;
            background.color = tempColor;
            yield return null;
        }

        background.color = new Color(background.color.r, background.color.g, 
            background.color.b, alpha > 0 ? alpha / 255f : alpha);
    }

    IEnumerator CloseWindowWithDelay(UIWindow wind, float delay)
    {
        yield return new WaitForSeconds(delay);
        CloseWindow(wind);
        BrightenBG();
        overlay.SetActive(true);
        windowOpened = false;
    }
}
