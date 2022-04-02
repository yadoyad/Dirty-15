using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffler : MonoBehaviour
{
    public string HelperText = "Нет текста для shuffler'а";
    public AbilitiesCtrl abilitiesCtrl;
    private FigureCtrl figureCtrl;
    private MatchFinder matchFinder;

    private void OnEnable() 
    {
        figureCtrl = FindObjectOfType<FigureCtrl>();
        matchFinder = FindObjectOfType<MatchFinder>();
    }

    public void Use()
    {
        if(abilitiesCtrl.ShufflerActivated(HelperText))
            Shuffle();
    }

    private void Shuffle()
    {
        abilitiesCtrl.fAnim.RotateAll();
        List<Figure> figs = figureCtrl.figures;
        foreach(Figure fig in figs)
        {
            if(fig.currentColor != FigureColor.Player)
            {
                FigureColor tmp = FigureColorizer.instance.GetRandomColor();
                fig.ColorSetup(tmp);
            }
        }
        GameJobsCtrl.instance.AddJob(matchFinder.FindMatches);
    }
}
