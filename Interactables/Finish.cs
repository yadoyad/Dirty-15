using UnityEngine;

public class Finish : Interactable
{
    public override void StuffOnPickUp()
    {
        // Debug.Log(gameObject.name + " got eaten by peepo");
        FinishCtrl.instance.LevelFinished();
        gameObject.SetActive(false);
    }
}
