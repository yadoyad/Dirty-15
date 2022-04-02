using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour
{
    public string HelperText = "Нет текста для щелчка";
    public AbilitiesCtrl abilitiesCtrl;
    public void Use()
    {
        abilitiesCtrl.SnapActivated(HelperText);
    }
}
