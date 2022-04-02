using System.Collections.Generic;
using UnityEngine;

public class InterCtrl : MonoBehaviour
{
    [Header("Родитель")]
    public GameObject interParent;

    [Header("Позиции")]
    [SerializeField] private Vector2 finishPosition;
    [SerializeField] private List<Vector2> pickUps = new List<Vector2>();

    [Header("Префабы")]
    public GameObject finishPrefab;
    public GameObject pickUp;

    private BoardCtrl bCtrl;
    private void OnEnable() 
    {
        bCtrl = FindObjectOfType<BoardCtrl>();
    }
    private void Start() 
    {
        PopulateIntField();
    }
    public void PopulateIntField()
    {
        List<Tile> tempTiles = bCtrl.myTiles;

        foreach(Tile tile in tempTiles)
        {
            if(tile.column == finishPosition.x)
            {
                if(tile.row == finishPosition.y)
                {
                    GameObject finish = Instantiate(finishPrefab, tile.tileObject.transform.position, Quaternion.identity);
                    finish.transform.parent = interParent.transform;
                    finish.name = "Finish";
                }
            }

            if(pickUps.Count > 0)
            {
                for(int i = 0; i < pickUps.Count; i++)
                {
                    if(tile.column == pickUps[i].x)
                    {
                        if(tile.row == pickUps[i].y)
                        {
                            GameObject pickUpObj = Instantiate(pickUp, tile.tileObject.transform.position, Quaternion.identity);
                            pickUpObj.transform.parent = interParent.transform;
                            pickUpObj.name = "PickUp_" + i;
                        }
                    }
                }
            }
        }
    }
    
}
