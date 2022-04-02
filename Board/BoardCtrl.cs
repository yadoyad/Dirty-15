using System.Collections.Generic;
using UnityEngine;

public enum TileStatus{ Empty, Figure, Stain }
public struct Tile
{
    public TileStatus status;
    public int column;
    public int row;
    public GameObject tileObject;

    public Tile(GameObject tile)
    {
        tileObject = tile;
        column = (int)tileObject.transform.position.x;
        row = (int)tileObject.transform.position.y;
        status = TileStatus.Empty;
    }
    
    public void SetState(TileStatus state)
    {
        status = state;
    }
}

public class BoardCtrl : MonoBehaviour
{
    public static BoardCtrl instance;
    public GameObject tileParentObj;
    public GameObject tilePrefab;
    //[HideInInspector] public List<GameObject> myField = new List<GameObject>();
    public List<Tile> myTiles = new List<Tile>();
    public List<GameObject> highlighters = new List<GameObject>();
    public int width {get; private set;} = 3;
    public int height {get; private set;} = 3;
    private List<Tile> highlightedTiles = new List<Tile>();

    private void Awake() 
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void OnEnable()
    {
        CreateField();
    }

    // private void Update() {
    //     if(Input.GetKeyDown(KeyCode.Space))
    //         ShowTileStatus();
    // }

    void CreateField()
    {
        //myField.Clear();
        
        for (int i = 0; i < width; i++)
        {
            for(int k = 0; k < height; k++)
            {
                Vector2 tempPosition = new Vector2(i, k);
                GameObject myTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity);
                myTile.transform.parent = tileParentObj.transform;
                myTile.name = "( " + i + ", " + k + " )";
                //myField.Add(myTile);

                Tile tempTile = new Tile(myTile);
                myTiles.Add(tempTile);
            }
        }
    }

    public void TileListUpdate(List<Tile> _tiles)
    {
        if(_tiles.Count == myTiles.Count)
        {
            myTiles = _tiles;
            // Debug.Log("MyTiles updated");
        }
        else
        {
            Debug.Log("Cant update tiles, lists aren't equal");
            Debug.Log("Previous list count: " + myTiles.Count + " and new is: " + _tiles.Count);
        }
            
    }

    //Используется при перемещении для апдейта состояния
    public void TwoTilesUpdate(Vector2 wasFig, Vector2 wasEmpty)
    {
        List<Tile> freshTiles = new List<Tile>();
        
        // Думал, что збс придумал, но в итоге не збс
        // Tile tTile = freshTiles.Find(x => x.column == wasFig.x && x.row == wasFig.y);
        // tTile.status = TileStatus.Empty;

        // Tile nTile = freshTiles.Find(x => x.column == wasEmpty.x && x.row == wasEmpty.y);
        // nTile.status = TileStatus.Figure;

        // freshTiles.Add(tTile);
        // freshTiles.Add(nTile);
        
        foreach(Tile tile in myTiles)
        {
            if(tile.column == wasFig.x)
            {
                if(tile.row == wasFig.y)
                {
                    Tile someTile = tile;
                    someTile.status = TileStatus.Empty;
                    // Debug.Log("Tile at " + someTile.column + " - " + someTile.row + " was NOT empty, but now is");
                    freshTiles.Add(someTile);
                }
            }
            if(tile.column == wasEmpty.x)
            {
                if(tile.row == wasEmpty.y)
                {
                    Tile someTile = tile;
                    someTile.status = TileStatus.Figure;
                    // Debug.Log("Tile at " + someTile.column + " - " + someTile.row + " was empty, but now is NOT");
                    freshTiles.Add(someTile);
                }
            }
        }

        foreach(Tile tile in myTiles)
        {
            int notApproved = 0;

            for(int i = 0; i < freshTiles.Count; i++)
            {
                if(tile.column == freshTiles[i].column && tile.row == freshTiles[i].row)
                    notApproved++;
            }

            if(notApproved == 0)
                freshTiles.Add(tile);
            
        }

        TileListUpdate(freshTiles);
    }

    public void MakeTileStained(Vector2 makeEmpty)
    {
        Tile tTile = myTiles.Find(x => x.column == makeEmpty.x && x.row == makeEmpty.y);
        tTile.status = TileStatus.Stain;

        List<Tile> freshTiles = new List<Tile>();
        freshTiles.Add(tTile);

        foreach(Tile tile in myTiles)
        {
            int notApproved = 0;

            if(tile.column == tTile.column && tile.row == tTile.row)
                notApproved++;

            if(notApproved == 0)
                freshTiles.Add(tile);
        }

        TileListUpdate(freshTiles);
    }

    public void MakeTileEmpty(Vector2 makeEmpty)
    {
        Tile tTile = myTiles.Find(x => x.column == makeEmpty.x && x.row == makeEmpty.y);
        tTile.status = TileStatus.Empty;

        List<Tile> freshTiles = new List<Tile>();
        freshTiles.Add(tTile);

        foreach(Tile tile in myTiles)
        {
            int notApproved = 0;

            if(tile.column == tTile.column && tile.row == tTile.row)
                notApproved++;

            if(notApproved == 0)
                freshTiles.Add(tile);
        }

        TileListUpdate(freshTiles);
    }

    public void HighlightTiles(List<Tile> _tiles)
    {
        if(_tiles.Count <= highlighters.Count)
        {
            List<Tile> tmpTiles = new List<Tile>();
            for(int i = 0; i < _tiles.Count; i++)
            {
                if(myTiles.Contains(_tiles[i]))
                {
                    int t = myTiles.IndexOf(_tiles[i]);
                    myTiles[t].tileObject.GetComponent<BoxCollider>().enabled = true;
                    highlighters[i].SetActive(true);
                    highlighters[i].transform.position = _tiles[i].tileObject.transform.position;
                    tmpTiles.Add(myTiles[t]);
                }
                else
                    Debug.Log("myTiles doesn't contain needed tile");
            }
            
            if(tmpTiles.Count > 0)
                highlightedTiles = tmpTiles;
        }
    }

    public void RemoveHighlights()
    {
        if(highlightedTiles.Count > 0)
        {
            for(int i = 0; i < highlightedTiles.Count; i++)
            {
                if(myTiles.Contains(highlightedTiles[i]))
                {
                    int t = myTiles.IndexOf(highlightedTiles[i]);
                    myTiles[t].tileObject.GetComponent<BoxCollider>().enabled = false;
                }
                // else
                //     Debug.Log("myTiles doesn't contain any highlighted tile");
            }

            foreach(GameObject hlight in highlighters)
            {
                hlight.SetActive(false);
            }
        }
    }

    void ShowTileStatus() //Для дебага
    {
        foreach(Tile tile in myTiles)
        {
            Debug.Log(tile.column + " - " + tile.row + " is " + tile.status);
        }
    }
}
