using System.Collections.Generic;
using UnityEngine;

public class ArrowCtrl : MonoBehaviour
{
    public GameObject upArrow;
    public GameObject downArrow;
    public GameObject leftArrow;
    public GameObject rightArrow;

    public void TurnOnArrows(List<Vector2> directions, Vector3 somePosition)
    {
        gameObject.transform.position = somePosition;

        foreach (Vector2 dir in directions)
        {
            if(dir.x == somePosition.x)
            {
                if(dir.y < somePosition.y)
                    downArrow.SetActive(true);
                if(dir.y > somePosition.y)
                    upArrow.SetActive(true);
            }
            if(dir.y == somePosition.y)
            {
                if(dir.x < somePosition.x)
                    leftArrow.SetActive(true);
                if(dir.x > somePosition.x)
                    rightArrow.SetActive(true);
            }
        }
    }

    public void TurnOffArrows()
    {
        upArrow.SetActive(false);
        downArrow.SetActive(false);
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
    }
}
