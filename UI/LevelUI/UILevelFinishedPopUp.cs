using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UILevelFinishedPopUp : MonoBehaviour
{
    public static UILevelFinishedPopUp instance;
    public TextMeshProUGUI movesTotal;
    public TextMeshProUGUI movesRequired;
    public TextMeshProUGUI figuresTotal;
    public TextMeshProUGUI figuresRequired;
    public Image movesJewel;
    public Image figuresJewel;
    public Color darkenedTint;

    private void Awake() 
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    public void SetUpTextFields()
    {
        SetUpTotals();
        SetUpRequireds();
        SetUpJewels();
    }

    private void SetUpRequireds()
    {
        var levelDescription = FindObjectOfType<LevelLoadManager>().currentLevel;
        var movesText = levelDescription.movesForBonus.ToString();
        var figuresText = levelDescription.figuresRequired.ToString();
        movesRequired.text = movesText;
        figuresRequired.text = figuresText;
    }

    private void SetUpTotals()
    {
        var movesText = MovesCtrl.instance.movesCount.ToString();
        var figuresText = FindObjectOfType<MatchFinder>().figuresMatched.ToString();
        movesTotal.text = movesText;
        figuresTotal.text = figuresText;
    }

    private void SetUpJewels()
    {
        var levelDescription = FindObjectOfType<LevelLoadManager>().currentLevel;
        var movesTotal = MovesCtrl.instance.movesCount;
        var movesRequired = levelDescription.movesForBonus;
        var figuresTotal = FindObjectOfType<MatchFinder>().figuresMatched;
        var figuresRequired = levelDescription.figuresRequired;

        if(movesTotal <= movesRequired)
        {
            // movesJewel.color = new Color(255,255,255);
            FindObjectOfType<JewelManager>().JewelCollected();
        }
        else
        {
            // Debug.Log("Setting dark jewel");
            movesJewel.color = darkenedTint;
        }

        if(figuresTotal >= figuresRequired)
        {
            // figuresJewel.color = new Color(255,255,255);
            FindObjectOfType<JewelManager>().JewelCollected();
        }
        else
        {
            // Debug.Log("Setting dark jewel");
            figuresJewel.color = darkenedTint;
        }
    }
}
