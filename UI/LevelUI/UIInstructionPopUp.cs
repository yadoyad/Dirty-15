using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIInstructionPopUp : MonoBehaviour
{
    public TextMeshProUGUI movesTotal;
    public TextMeshProUGUI figsTotal;

    private void Start() 
    {
        SetupTextFields();
    }

    private void SetupTextFields()
    {
        var moves = LevelLoadManager.instance.currentLevel.movesForBonus.ToString();
        var figs = LevelLoadManager.instance.currentLevel.figuresRequired.ToString();

        movesTotal.text = moves;
        figsTotal.text = figs;
    }
}
