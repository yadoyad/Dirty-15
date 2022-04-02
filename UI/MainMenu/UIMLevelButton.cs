using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMLevelButton : MonoBehaviour
{
    public LevelDescription level;

    public void ButtonPress()
    {
        //Тут должен быть вызов дополнительного popup с подробным описанием уровня
        //Но пока что обойдемся обычной загрузкой уровня

        LevelLoadManager.instance.LoadLevelFromMenu(level);
    }
}
