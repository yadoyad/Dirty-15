using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public class BacklightCtrl : MonoBehaviour
{
    public static BacklightCtrl instance;

    [Header("Настройки анимации")]
    [SerializeField] private float animationTime = 1f;
    [SerializeField, Range(0, 1)] private float intensityRange = 0.1f;
    [SerializeField] private Ease animationEase = Ease.Linear;

    [Header("Цвета")]
    public Color Blue;
    public Color Gold;
    public Color Green;
    public Color Orange;
    public Color Purple;
    public Color Red;
    public Color Grey;
    public Color Player;
    private Light2D myLight;
    private FigureCtrl figureCtrl;
    private bool animationInProgress;
    private event Action OnAnimationEnd;
    private void Awake() 
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable() 
    {
        myLight = GetComponent<Light2D>();
        figureCtrl = FindObjectOfType<FigureCtrl>();
    }

    private void Start() 
    {
        figureCtrl.OnFigureSelected += LightUp;
        figureCtrl.OnFigureDeselected += ShutTheLight;
    }

    private void OnDisable() 
    {
        figureCtrl.OnFigureSelected -= LightUp;
        figureCtrl.OnFigureDeselected -= ShutTheLight;
    }

    public void LightUp()
    {
        OnAnimationEnd -= LightUp;
        
        if(animationInProgress)
            OnAnimationEnd += LightUp;
        else
        {
            animationInProgress = true;

            Figure tmp = figureCtrl.selectedFigure.GetComponent<Figure>();
            ChangeColor(tmp.currentColor);

            float someFloat = myLight.intensity;
            DOTween.To(() => myLight.intensity, x => myLight.intensity = x, intensityRange, animationTime * .5f)
                .SetEase(animationEase)
                .OnComplete(AnimationEnd);
        }
    }

    public void ShutTheLight()
    {
        OnAnimationEnd -= ShutTheLight;

        if(animationInProgress)
            OnAnimationEnd += ShutTheLight;
        else
        {
            animationInProgress = true;
            DOTween.To(() => myLight.intensity, x => myLight.intensity = x, 0f, animationTime)
                .SetEase(animationEase)
                .OnComplete(AnimationEnd);
        }
    }

    private void AnimationEnd()
    {
        animationInProgress = false;
        OnAnimationEnd?.Invoke();
    }

    public void ChangeColor(FigureColor targetColor)
    {
        switch(targetColor)
        {
            case FigureColor.Blue:
            myLight.color = Blue;
            break;

            case FigureColor.Gold:
            myLight.color = Gold;
            break;

            case FigureColor.Green:
            myLight.color = Green;
            break;

            case FigureColor.Orange:
            myLight.color = Orange;
            break;

            case FigureColor.Purple:
            myLight.color = Purple;
            break;

            case FigureColor.Red:
            myLight.color = Red;
            break;

            case FigureColor.Grey:
            myLight.color = Grey;
            break;

            case FigureColor.Player:
            myLight.color = Player;
            break;

            default:
            myLight.color = Color.white;
            break;
        }
    } 
}
