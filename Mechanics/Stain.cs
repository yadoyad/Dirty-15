using UnityEngine;

public enum StainColor { Blue, Gold, Green, Orange, Purple, Red, Grey }

[RequireComponent(typeof(SpriteRenderer))]
public class Stain : MonoBehaviour
{
    public Sprite Blue;
    public Sprite Gold;
    public Sprite Green;
    public Sprite Orange;
    public Sprite Purple;
    public Sprite Red;
    public StainColor currentColor {get; private set;}
    public SpriteRenderer sr;
    private int appearMove = 0;
    private int stainCooldown = 1;

    public void StainInit()
    {
        appearMove = MovesCtrl.instance.movesCount;
        MovesCtrl.instance.OnCountChange += CheckForMoves;
    }

    private void OnDisable() {
        MovesCtrl.instance.OnCountChange -= CheckForMoves;
    }

    public void ColorSetup(FigureColor col)
    {
        switch(col)
        {
            case FigureColor.Blue:
                currentColor = StainColor.Blue;
                sr.sprite = Blue;
                break;
            case FigureColor.Gold:
                currentColor = StainColor.Gold;
                sr.sprite = Gold;
                break;
            case FigureColor.Green:
                currentColor = StainColor.Green;
                sr.sprite = Green;
                break;
            case FigureColor.Orange:
                currentColor = StainColor.Orange;
                sr.sprite = Orange;
                break;    
            case FigureColor.Purple:
                currentColor = StainColor.Purple;
                sr.sprite = Purple;
                break;  
            case FigureColor.Red:
                currentColor = StainColor.Red;
                sr.sprite = Red;
                break;                                          
        }
    }

    public void CheckForMoves()
    {
        if(MovesCtrl.instance.movesCount > appearMove + stainCooldown)
        {
            BoardCtrl.instance.MakeTileEmpty(new Vector2(transform.position.x, transform.position.y));
            Destroy(gameObject);
        }
    }
}
