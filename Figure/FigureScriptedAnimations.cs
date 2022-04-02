using System.Collections;
using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class FigureScriptedAnimations : MonoBehaviour
{
    public static FigureScriptedAnimations instance;
    [Header("Передвижение и селекция")]
    [SerializeField] float scaleOffset = 1.2f;
    [SerializeField] float sizeChangeTime = 1f;
    [SerializeField] float movementAnimationTime = 1f;
    [HideInInspector] public bool movementInProgress { get; private set; } = false;
    [Header("Пошатывание")]
    [SerializeField] float duration = .5f;
    [SerializeField] float strength = .2f;
    [SerializeField] int vibrato = 10;
    [SerializeField] int randomness = 40;

    [Header("Ротация")]
    [SerializeField] float rotationDuration = .5f;
    [SerializeField] float rotationStrength = .2f;
    [SerializeField] int rotationVibrato = 10;
    [SerializeField] int rotationRandomness = 40;

    public event Action OnMovementEnd;
    public event Action OnMovementStart;

    private FigureCtrl figureCtrl;
    private IEnumerator currentRoutine;

    private void Awake() 
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable() 
    {
        figureCtrl = FindObjectOfType<FigureCtrl>();
    }
    #region Передвижение и селекция
    public void Enlarge(GameObject fig)
    {
        currentRoutine = EnlargeFigure(fig);
        StartCoroutine(currentRoutine);
    }
    IEnumerator EnlargeFigure(GameObject fig)
    {
        if(fig.transform.localScale.x == 1)
        {
            Vector3 originalScale = fig.transform.localScale;
            Vector3 enlargedScale = new Vector3(originalScale.x * scaleOffset, originalScale.y * scaleOffset, originalScale.z * scaleOffset);
            float timeElapsed = 0f;

            while(timeElapsed < sizeChangeTime)
            {
                timeElapsed += Time.deltaTime;
                Vector3 tempScale = Vector3.Lerp(originalScale, enlargedScale, timeElapsed / sizeChangeTime);
                fig.transform.localScale = tempScale;
                yield return null;
            }

            fig.transform.localScale = enlargedScale;
        }
        else
            Debug.Log("Попытка увеличить фигуру нестандартного размера");
    }

    public void Reduce(GameObject fig)
    {
        currentRoutine = ReduceFigure(fig);
        StartCoroutine(currentRoutine);
    }

    IEnumerator ReduceFigure(GameObject fig)
    {
        if(fig.transform.localScale.x == scaleOffset)
        {
            Vector3 enlargedScale = fig.transform.localScale;
            Vector3 originalScale = new Vector3(enlargedScale.x / scaleOffset, enlargedScale.y / scaleOffset, enlargedScale.z / scaleOffset);
            float timeElapsed = 0f;

            while(timeElapsed < sizeChangeTime)
            {
                timeElapsed += Time.deltaTime;
                Vector3 tempScale = Vector3.Lerp(enlargedScale, originalScale, timeElapsed / sizeChangeTime);
                fig.transform.localScale = tempScale;
                yield return null;
            }

            fig.transform.localScale = originalScale;
        }
    }

    public void Move(GameObject fig, GameObject tile)
    {
        currentRoutine = MoveFigure(fig, tile);
        StartCoroutine(currentRoutine);
    }
    IEnumerator MoveFigure(GameObject fig, GameObject tile)
    {
        Vector3 currentPosition = fig.transform.position;
        Vector3 destination = tile.transform.position;
        
        float timeElapsed = 0f;
        OnMovementStart?.Invoke();

        while(timeElapsed < movementAnimationTime)
        {
            timeElapsed += Time.deltaTime;
            movementInProgress = true;
            Vector3 tempPos = Vector3.Lerp(currentPosition, destination, timeElapsed / movementAnimationTime);
            fig.transform.position = tempPos;
            yield return null;
        }

        OnMovementEnd?.Invoke();
        fig.transform.position = destination;
        movementInProgress = false;
    }

    #endregion

    #region Пошатывание
    public void Shake(GameObject fig)
    {
        var figPos = fig.transform.position;

        if(fig.tag == "Player")
        {
            var f = fig.GetComponent<Figure>();
            f.GetMad();
        }

        fig.transform.DOShakePosition(duration, strength, vibrato, randomness, false, true)
            .OnPlay(() => movementInProgress = true)
            .OnComplete(() => fig.transform.position = figPos)
            .OnComplete(() => movementInProgress = false);
    }

    public void ShakeAll()
    {
        List<Figure> figs = figureCtrl.figures;
        foreach(Figure fig in figs)
        {
            if(fig.currentColor != FigureColor.Player)
                Shake(fig.gameObject);
        }
    }

    #endregion
    private void RotateFigure(GameObject fig)
    {
        fig.transform.DOShakeRotation(rotationDuration, rotationStrength, rotationVibrato, rotationRandomness, true);
    }
    public void RotateAll()
    {
        List<Figure> figs = figureCtrl.figures;
        foreach(Figure fig in figs)
        {
            if(fig.currentColor != FigureColor.Player)
                RotateFigure(fig.gameObject);
        }
    }
}
