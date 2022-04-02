using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            Figure tFig = other.gameObject.GetComponent<Figure>();
            if(tFig.currentColor == FigureColor.Player)
            {
                StuffOnPickUp();
            }
        }
    }

    public abstract void StuffOnPickUp();
}
