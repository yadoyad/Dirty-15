using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    public bool searchin {get; private set;} = false;
    public int figuresMatched {get; private set;} = 0;
    FigureCtrl fCtrl;
    BoardCtrl bCtrl;
    GameJobsCtrl jobsCtrl;
    JewelManager jewelManager;

    private void OnEnable() 
    {
        fCtrl = FindObjectOfType<FigureCtrl>();
        bCtrl = FindObjectOfType<BoardCtrl>();
        jobsCtrl = FindObjectOfType<GameJobsCtrl>();
        jewelManager = FindObjectOfType<JewelManager>();
    }
    private void Start() 
    {
        figuresMatched = 0;
        AddFinderAtStart();
        FigureScriptedAnimations.instance.OnMovementEnd += AddFinder;
    }

    private void AddFinder()
    {
        GameJobsCtrl.instance.AddJob(FindMatches);
    }

    private void AddFinderAtStart()
    {
        StartCoroutine(OneTickLater());
    }

    IEnumerator OneTickLater()
    {
        yield return null;
        GameJobsCtrl.instance.AddJob(FindMatchesAtStart);
    }

    public void MatchCountInc() => figuresMatched++;

    public void FindMatches()
    {
        searchin = true;
        List<Figure> tmpList = fCtrl.figures;

        foreach(Figure fig in tmpList)
        {
            if(!fig.isMatched)
            {
                List<Figure> matches = FindMatches(fig, tmpList);
                if(matches.Count > 0)
                {
                    foreach(Figure match in matches)
                    {
                        match.GotMatched();
                        // figuresMatched++;
                    }
                    var kostyl = GameObject.FindGameObjectWithTag("Player");
                    kostyl.GetComponent<Figure>().GetSmart();
                    jewelManager.JewelCollected();
                }
            }
        }
        searchin = false;
    }

    public void FindMatchesAtStart()
    {
        bool result = false;
        searchin = true;

        List<Figure> tmpList = fCtrl.figures;

        foreach(Figure fig in tmpList)
        {
            if(!fig.isMatched)
            {
                List<Figure> matches = FindMatches(fig, tmpList);
                if(matches.Count > 0)
                {
                    foreach(Figure match in matches)
                    {
                        FigureColor tColor = FigureColorizer.instance.GetRandomColor();
                        match.ColorSetup(tColor);
                    }
                    result = true;
                }
            }
        }
        searchin = false;

        if(result)
            FindMatchesAtStart();
    }

    //Возвращает список фигур, у которых есть совпадение
    private List<Figure> FindMatches(Figure fig, List<Figure> figs)
    {
        List<Figure> matchedFigures = new List<Figure>();

        Vector2 pos = fig.tilePosition;

        if(pos.x > 0 && pos.x < bCtrl.width - 1)
        {
            Figure horLeft = figs.Find(x => x.tilePosition == new Vector2(pos.x - 1, pos.y));
            Figure horRight = figs.Find(x => x.tilePosition == new Vector2(pos.x + 1, pos.y));
            if((horLeft && horLeft.currentColor == fig.currentColor) && (horRight && horRight.currentColor == fig.currentColor))
            {
                if(!horLeft.isMatched && !horRight.isMatched)
                {
                    matchedFigures.Add(fig);
                    matchedFigures.Add(horRight);
                    matchedFigures.Add(horLeft);
                }
            }
        }
        if(pos.y > 0 && pos.y < bCtrl.height - 1)
        {
            Figure verUp = figs.Find(x => x.tilePosition == new Vector2(pos.x, pos.y + 1));
            Figure verDown = figs.Find(x => x.tilePosition == new Vector2(pos.x, pos.y - 1));
            if((verUp && verUp.currentColor == fig.currentColor) && (verDown && verDown.currentColor == fig.currentColor))
            {
                if(!verUp.isMatched && !verDown.isMatched)
                {
                    matchedFigures.Add(fig);
                    matchedFigures.Add(verUp);
                    matchedFigures.Add(verDown);
                }
            }
        }

        return matchedFigures;
    }
}
