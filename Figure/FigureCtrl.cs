using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class FigureCtrl : MonoBehaviour
{
    public static FigureCtrl instance;
    [HideInInspector] public GameObject selectedFigure {get; private set;} = null;
    [HideInInspector] public List<Figure> figures {get; private set;} = new List<Figure>();
    [SerializeField] private List<Vector2> emptyTiles = new List<Vector2>();
    [SerializeField] private List<Vector2> lockedFigures = new List<Vector2>();
    [SerializeField] private Vector2 playerPosition = new Vector2(0, 0);
    [SerializeField] private int amountOfColors = 6;
    public int colorAmount {get; private set;}
    private FigureScriptedAnimations fAnim;
    private FieldPopulator fp;
    private BoardCtrl bCtrl;
    private ArrowCtrl arrowCtrl;
    private List<Tile> possibleMoves = new List<Tile>();
    public event Action OnFigureSelected;
    public event Action OnFigureDeselected;

    private void Awake() 
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        colorAmount = amountOfColors;
    }
    private void OnEnable() 
    {
        fp = FindObjectOfType<FieldPopulator>();
        arrowCtrl = FindObjectOfType<ArrowCtrl>();
        fAnim = FindObjectOfType<FigureScriptedAnimations>();
        bCtrl = FindObjectOfType<BoardCtrl>();
    }

    void Start()
    {
        fp.PopulateField(emptyTiles, lockedFigures, playerPosition);
    }

    #region SELECTION
    //Метод вызывается только при ожидании повторного касания
    public bool SelectFigure(GameObject figure)
    {   
        // Debug.Log("SL fired");  
        bool result = false;

        if(selectedFigure == figure)
        {
            result = true;
            DeselectFigure();
            return result;
        }
        else
            DeselectFigure();

        selectedFigure = figure;

        FindMoves();

        if(possibleMoves.Count > 0)
        {
            OnFigureSelected?.Invoke();
            if(possibleMoves.Count > 1)
            {
                fAnim.Enlarge(figure);
                bCtrl.HighlightTiles(possibleMoves);
            }
            else if(possibleMoves.Count == 1)
                TileSelected();
        }

        return result;
    }

    //Вызывается при первом касании и возвращает количество ходов
    public int SelectFigure(GameObject figure, bool firstTry)
    {
        int result = 0;

        selectedFigure = figure;

        FindMoves();

        if(possibleMoves.Count > 0)
        {
            result = possibleMoves.Count;
            OnFigureSelected?.Invoke();

            if(possibleMoves.Count > 1)
            {
                fAnim.Enlarge(figure);
                bCtrl.HighlightTiles(possibleMoves);
            }
            else if(possibleMoves.Count == 1)
                TileSelected();
        }
        else
        {
            //Ссылка на figureScriptedAnimations
            if(!fAnim.movementInProgress)
                fAnim.Shake(figure);
        }

        return result;
    }

    public void DeselectFigure()
    {
        OnFigureDeselected?.Invoke();
        if(selectedFigure != null)
            fAnim.Reduce(selectedFigure);

        selectedFigure = null;
        bCtrl.RemoveHighlights();
    }
    #endregion

    #region MOVEMENT

    public void MoveFigure() => MoveFigure(possibleMoves[0].tileObject);

    public void MoveFigure(GameObject tile)
    {
        // Debug.Log("MoveFigure fired");
        Vector2 figPos = selectedFigure.transform.position;
        Vector2 tilePos = tile.transform.position;

        fAnim.Move(selectedFigure, tile);
        
        selectedFigure.GetComponent<Figure>().TilePositionSetup(tilePos);

        DeselectFigure();
        bCtrl.TwoTilesUpdate(figPos, tilePos);
    }
    #endregion

    public FigureColor GetRandomColor() //Потом в отдельный класс для назначения цветов не три в ряд
    {
        FigureColor someColor = FigureColor.Grey;

        int figCol = UnityEngine.Random.Range(1, amountOfColors + 1);

        switch (figCol)
        {
            case 1:
                someColor = FigureColor.Blue;
                break;
            case 2:
                someColor = FigureColor.Red;
                break;
            case 3:
                someColor = FigureColor.Green;
                break;
            case 4:
                someColor = FigureColor.Orange;
                break;
            case 5:
                someColor = FigureColor.Purple;
                break;
            case 6:
                someColor = FigureColor.Gold;
                break;
        }

        return someColor;
    }
    public void AddFigure(GameObject fig)
    {
        Figure tmpFig = fig.GetComponent<Figure>();
        if(!figures.Contains(tmpFig))
            figures.Add(tmpFig);
        else
            Debug.Log("Trying to add clone to figures list");
    }

    //В этих трех методах порядок бы навести, делают почти одно и то же, но у меня спидкодинг, штоподелать
    public void RemoveFigure(Figure fig) //Раз
    {
        bCtrl.MakeTileStained(fig.tilePosition);
    }
    public void SnapFigure(Figure fig)  //Дваз
    {
        bCtrl.MakeTileEmpty(fig.tilePosition);
    }
    public void RemoveFigure(GameObject fig) //Триз
    {
        Figure f = fig.GetComponent<Figure>();
        f.SnapMe();
    }
    public void TileSelected(GameObject tile)
    {
        int tColumn = (int)tile.transform.position.x;
        int tRow = (int)tile.transform.position.y;
        Tile tempTile = new Tile();

        foreach(Tile t in possibleMoves)
        {
            if(t.column == tColumn)
            {
                if(t.row == tRow)
                    tempTile = t;
            }
        }

        if(tempTile.row == tRow && tempTile.column == tColumn)
        {
            possibleMoves.Clear();
            possibleMoves.Add(tempTile);
            TileSelected();
        }
        else
        {
            Debug.Log("Something went wrong in TileSelected");
        }
    }
    
    public void TileSelected() => GameJobsCtrl.instance.AddJob(MoveFigure);
    private void FindMoves()
    {
        possibleMoves.Clear();

        int tColumn = (int)selectedFigure.transform.position.x;
        int tRow = (int)selectedFigure.transform.position.y;

        List<Tile> tTiles = FindObjectOfType<BoardCtrl>().myTiles;

        foreach(Tile tile in tTiles)
        {
            if(tile.column == tColumn)
            {
                if(tile.row == (tRow-1) || tile.row == (tRow+1))
                {
                    if(tile.status == TileStatus.Empty) 
                    {
                        possibleMoves.Add(tile);
                    }
                }
            }
            if(tile.row == tRow)
            {
                if(tile.column == (tColumn - 1) || tile.column == (tColumn + 1))
                {
                    if(tile.status == TileStatus.Empty) 
                    {
                        possibleMoves.Add(tile);
                    }
                }
            }
        }
    }
}
