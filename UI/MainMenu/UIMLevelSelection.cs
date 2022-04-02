using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIScriptedAnimations))]
public class UIMLevelSelection : MonoBehaviour
{
    public Vector3 offset;
    private UIScriptedAnimations levelAnimations;
    private RectTransform rTrans;
    private void OnEnable() 
    {
        levelAnimations = GetComponent<UIScriptedAnimations>();
        rTrans = GetComponent<RectTransform>();
    }

    public void MoveLeft()
    {
        levelAnimations.Move(rTrans.localPosition + offset);
    }

    public void MoveRight()
    {
        levelAnimations.Move(rTrans.localPosition - offset);
    }
}
