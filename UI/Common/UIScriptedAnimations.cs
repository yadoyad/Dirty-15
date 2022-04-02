using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class UIScriptedAnimations : MonoBehaviour
{
    [Header("Настройки анимации")]
    public float animationTime = 1f;
    [SerializeField] private Ease _moveEase = Ease.Linear;

    [Header("Изменяемые параметры объекта")]
    [SerializeField] private Vector3 hidingPosition = Vector3.zero;
    [SerializeField] private Vector3 targetPosition = Vector3.zero;
    [SerializeField] private float scaleMultiplier = 1f;

    private RectTransform rTrans;

    private void OnEnable() 
    {
        rTrans = GetComponent<RectTransform>();
    }

    public void Move()
    {
        rTrans.localPosition = -hidingPosition;
        rTrans.DOLocalMove(targetPosition, animationTime).SetEase(_moveEase);
    }

    public void Move(Vector3 target)
    {
        rTrans.DOLocalMove(target, animationTime).SetEase(_moveEase);
    }
    public void MoveBack()
    {
        rTrans.DOLocalMove(hidingPosition, animationTime).SetEase(_moveEase);
    }
    public void MoveWithScale()
    {
        throw new NotImplementedException();
    }
}
