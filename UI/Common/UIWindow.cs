using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIWindow : MonoBehaviour
{
    [Header("Отключаемые компоненты")]
    public List<GameObject> comps = new List<GameObject>();

    [Header("Текстовые поля")]
    public TextMeshProUGUI Header;
    public TextMeshProUGUI Body;

    private UIScriptedAnimations anims;
    private string HeaderText;
    private string BodyText;

    private void OnEnable() 
    {
        anims = GetComponent<UIScriptedAnimations>();
    }

    public void OpenWindow()
    {
        foreach(GameObject comp in comps)
        {
            if(!comp.activeSelf)
                comp.SetActive(true);
        }
        anims.Move();
    }

    public void CloseWindow()
    {
        anims.MoveBack();
    }
}
