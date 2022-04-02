using UnityEngine;
using TMPro;

public class UIJewelCounter : MonoBehaviour
{
    public TextMeshProUGUI counter;
    private JewelManager jewelManager;

    private void OnEnable() 
    {
        jewelManager = FindObjectOfType<JewelManager>();
    }

    public void UpdateCounterText()
    {
        counter.text = jewelManager.jewelAmount.ToString();
    }
}
