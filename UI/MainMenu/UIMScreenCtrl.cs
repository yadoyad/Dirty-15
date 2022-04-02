using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMScreenCtrl : MonoBehaviour
{
    public UIScriptedAnimations mainMenu;
    public UIScriptedAnimations levelsMenu;
    public UIScriptedAnimations settingsMenu;

    [Header("Настройки анимации")]
    [Range(0f, 1f)] public float animationDelay = 0.5f;

    public void MainMenu_PlayButton()
    {
        StartCoroutine(ScreenTransition(mainMenu, levelsMenu));
    }

    public void MainMenu_SettingsButton()
    {
        StartCoroutine(ScreenTransition(mainMenu, settingsMenu));
    }

    public void SettingsMenu_BackButton()
    {
        StartCoroutine(ScreenTransition(settingsMenu, mainMenu));
    }

    public void LevelsMenu_BackButton()
    {
        StartCoroutine(ScreenTransition(levelsMenu, mainMenu));
    }

    IEnumerator ScreenTransition(UIScriptedAnimations current, UIScriptedAnimations target)
    {
        current.MoveBack();
        yield return new WaitForSeconds(animationDelay);
        target.Move();
    }
}
