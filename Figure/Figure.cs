using UnityEngine;
using System.Collections;
using System;

public enum FigureColor { Blue, Gold, Green, Orange, Purple, Red, Grey, Player }
public class Figure : MonoBehaviour
{
    [SerializeField] private float EmoteCD = .4f;
    public GameObject lockSign;
    public GameObject stainPrefab;
    public Sprite Blue;
    public Sprite Gold;
    public Sprite Green;
    public Sprite Orange;
    public Sprite Purple;
    public Sprite Red;
    public Sprite Grey;
    public Sprite Player;
    public Sprite PlayerMad;
    public Sprite PlayerSmart;
    public Vector2 tilePosition {get; private set;}
    public FigureColor currentColor {get; private set;}
    public bool isMatched {get; private set;} = false;
    SpriteRenderer spriteRenderer;
    FigureCtrl fCtrl;

    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() 
    {
        fCtrl = FindObjectOfType<FigureCtrl>();
    }
    private void Start() 
    {
        StartCoroutine(TickAfterStart());
    }

    IEnumerator TickAfterStart()
    {
        yield return null;
        if(gameObject.tag != "Player")
        {
            FigureColor tempCol = FigureColorizer.instance.GetRandomColor();
            ColorSetup(tempCol);
        }
        
    }

    public void ColorSetup(FigureColor col)
    {
        currentColor = col;

        switch(currentColor)
        {
            case FigureColor.Blue:
                spriteRenderer.sprite = Blue;
                break;
            case FigureColor.Gold:
                spriteRenderer.sprite = Gold;
                break;
            case FigureColor.Green:
                spriteRenderer.sprite = Green;
                break;
            case FigureColor.Orange:
                spriteRenderer.sprite = Orange;
                break;
            case FigureColor.Purple:
                spriteRenderer.sprite = Purple;
                break;
            case FigureColor.Red:
                spriteRenderer.sprite = Red;
                break;
            case FigureColor.Player:
                spriteRenderer.sprite = Player;
                gameObject.tag = "Player";
                break;
            default:
                spriteRenderer.sprite = Grey;
                break;
        }
    }

    public void LockFigure()
    {
        lockSign.SetActive(true);
        gameObject.tag = "NotSelectable";
    }

    public void TilePositionSetup(Vector2 pos) => tilePosition = pos;
    public void GotMatched()
    {
        isMatched = true;
        RemoveMe();
    }

    public void GetMad()
    {
        StartCoroutine(ChangeEmote(PlayerMad));
    }

    public void GetSmart()
    {
        StartCoroutine(ChangeEmote(PlayerSmart));
    }

    IEnumerator ChangeEmote(Sprite emote)
    {
        spriteRenderer.sprite = emote;
        yield return new WaitForSeconds(EmoteCD);
        spriteRenderer.sprite = Player;
    }

    public void RemoveMe()
    {        
        CreateStain();
        fCtrl.RemoveFigure(this);
        gameObject.SetActive(false);
        FindObjectOfType<MatchFinder>().MatchCountInc();
    }

    public void SnapMe()
    {
        fCtrl.SnapFigure(this);
        gameObject.SetActive(false);
        FindObjectOfType<MatchFinder>().MatchCountInc();
    }

    private void CreateStain()
    {
        var myStain = Instantiate(stainPrefab, transform.position, Quaternion.identity);
        Stain tStain = myStain.GetComponent<Stain>();
        tStain.ColorSetup(currentColor);
        tStain.StainInit();
    }
    public void Unmatch() => isMatched = false;
}
