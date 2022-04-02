using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FigureColorizer : MonoBehaviour
{
    public static FigureColorizer instance;
    public int amountOfColors = 6;
    public int totalColors = 6;
    private List<int> palette = new List<int>();

    private void Awake() 
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable() 
    {
        palette = GetColorPalette();
    }

    private List<int> GetColorPalette()
    {
        List<int> colorPalette = Enumerable.Range(1, totalColors).ToList();
        int tColor;

        for (int i = 0; i < totalColors - amountOfColors; i++)
        {
            tColor = UnityEngine.Random.Range(0, colorPalette.Count);
            colorPalette.RemoveAt(tColor);
        }
        
        return colorPalette;
    }

    private int GetRColor() => UnityEngine.Random.Range(1, 7);
    public FigureColor GetRandomColor() //Потом в отдельный класс для назначения цветов не три в ряд
    {
        FigureColor someColor = FigureColor.Grey;

        int figCol = palette[UnityEngine.Random.Range(0, palette.Count)];

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
}
