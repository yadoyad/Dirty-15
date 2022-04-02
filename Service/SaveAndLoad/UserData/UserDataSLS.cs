using UnityEngine;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Serializers;

public class UserDataSLS : MonoBehaviour
{
    public static UserDataSLS instance;
    public UserData userData {get; private set;}
    private ISaveGameSerializer mySerializer = new SaveGameBinarySerializer();
    private const string identifier = "c308_lkx4";

    private void Awake() 
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable() 
    {
        LoadUD();
    }

    public void UpdateJewelData(int amount)
    {
        userData.jewelAmount = amount;
        SaveUD();
    }
    public void FlushUD()
    {
        userData = new UserData();
        SaveUD();
        Debug.Log("UsedData deleted");
    }
    public void SaveUD()
    {
        SaveGame.Save<UserData>(identifier, userData, mySerializer);
        // Debug.Log("Saved UD");
    }

    public void LoadUD()
    {
        // Debug.Log("Loading UserData...");
        if(SaveGame.Exists(identifier))
        {
            userData = SaveGame.Load<UserData>(identifier, new UserData(), mySerializer);
        }
        else
        {
            // Debug.Log("Couldn't find save file, creating new");
            userData = new UserData();
            SaveUD();
        }
    }
}
