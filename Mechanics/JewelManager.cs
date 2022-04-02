using UnityEngine;

public class JewelManager : MonoBehaviour
{
    public int jewelAmount { get; private set; } = 0;
    private UIJewelCounter jewelCounter;

    private void OnEnable() 
    {
        jewelCounter = FindObjectOfType<UIJewelCounter>();
    }

    private void Start() 
    {
        SetJewelAmount(UserDataSLS.instance.userData.jewelAmount);
    }

    private void SetJewelAmount(int amount)
    {
        jewelAmount = amount;

        if(amount == 0)
            Debug.Log("SetJewelAmount сработал на ноль");

        UpdateJewelText();
    }

    public void JewelCollected()
    {
        jewelAmount++;
        SaveJewelAmount();
        UpdateJewelText();
    }

    public void JewelSpent(int amount)
    {
        if(jewelAmount >= amount)
        {
            jewelAmount -= amount;
        }
        else
        {
            Debug.Log("Ошибка количества в JewelSpent");
            jewelAmount = 0;
        }
        SaveJewelAmount();
        UpdateJewelText();
    }

    private void UpdateJewelText()
    {
        jewelCounter.UpdateCounterText();
    }

    private void SaveJewelAmount()
    {
        UserDataSLS.instance.UpdateJewelData(jewelAmount);
    }
}
