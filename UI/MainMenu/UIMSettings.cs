using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMSettings : MonoBehaviour
{
    public void FlushSaveFile()
    {
        UserDataSLS.instance.FlushUD();
    }
}
