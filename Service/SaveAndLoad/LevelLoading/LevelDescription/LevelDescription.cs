using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelDescription", menuName = "Описание уровня")]
public class LevelDescription : ScriptableObject
{
    public int levelBuildIndex = 0;
    public int movesRequired = 0;
    public int figuresRequired = 0;
    public int movesForBonus = 0;
}
