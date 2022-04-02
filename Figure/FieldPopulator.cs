using System.Collections.Generic;
using UnityEngine;

public class FieldPopulator : MonoBehaviour
{
    public GameObject figParentObject;
    public GameObject figurePrefab;
    public GameObject finishPrefab;
    private BoardCtrl bCtrl;
    private FigureCtrl fCtrl;

    private void OnEnable() 
    {
        bCtrl = FindObjectOfType<BoardCtrl>();
        fCtrl = FindObjectOfType<FigureCtrl>();
    }

    public void PopulateField(List<Vector2> emptyTiles, List<Vector2> lockedF, Vector2 playerPos)
    {
        List<Tile> oldTiles = bCtrl.myTiles;
        List<Tile> freshTiles = new List<Tile>();

        for(int i=0; i<emptyTiles.Count; i++)
        {
            foreach(Tile tile in oldTiles)
            {
                if(tile.column == (int)emptyTiles[i].x)
                {
                    if(tile.row == (int)emptyTiles[i].y)
                    {
                        Tile someTile = tile;
                        someTile.status = TileStatus.Empty;
                        freshTiles.Add(someTile);
                    }
                }
            }
        }

        foreach(Tile tile in oldTiles)
        {
            if(!freshTiles.Contains(tile))
            {
                Tile tempTile = tile;

                GameObject figure = Instantiate(figurePrefab, tile.tileObject.transform.position, Quaternion.identity);
                figure.transform.parent = figParentObject.transform;
                figure.name = tile.tileObject.name;
                figure.GetComponent<Figure>().TilePositionSetup(new Vector2(tile.column, tile.row));

                if(tile.column == playerPos.x && tile.row == playerPos.y)
                {
                    FigureColor pColor = FigureColor.Player;
                    figure.GetComponent<Figure>().ColorSetup(pColor);
                }

                for (int i = 0; i < lockedF.Count; i++)
                {
                    if(tile.column == lockedF[i].x && tile.row == lockedF[i].y)
                        figure.GetComponent<Figure>().LockFigure();
                }

                fCtrl.AddFigure(figure);
                
                tempTile.status = TileStatus.Figure;

                freshTiles.Add(tempTile);

            }
        }

        bCtrl.TileListUpdate(freshTiles);
    }
}
