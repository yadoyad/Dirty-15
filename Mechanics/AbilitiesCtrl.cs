using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilitiesCtrl : MonoBehaviour
{
    public static AbilitiesCtrl instance;
    [Header("Активные абилки")]
    [SerializeField] bool snapEnabled;
    [SerializeField] bool shuffleEnabled;
    [Header("Настройки абилок")]
    public Button snapBtn;
    public Button shuffleBtn;
    public float GCD = .4f;
    public TextMeshProUGUI textHelper;
    public float textFadeAnimationTime = 1f;
    public string notEnough = "Не хватает кристаллов";
    public int SnapCost = 0;
    public int ShuffleCost = 0;
    public bool SnapActive {get; private set;} = false;
    [HideInInspector] public FigureScriptedAnimations fAnim {get; private set;}
    private InputManager inputManager;
    private JewelManager jm;
    private IEnumerator currentRoutine;

    private void Awake() 
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable() 
    {
        inputManager = FindObjectOfType<InputManager>();
        jm = FindObjectOfType<JewelManager>();
        fAnim = FindObjectOfType<FigureScriptedAnimations>();
    }

    private void Start() 
    {
        snapBtn.interactable = snapEnabled;
        shuffleBtn.interactable = shuffleEnabled;
    }
    

    public void SnapActivated(string s)
    {
        if(jm.jewelAmount >= SnapCost)
        {
            SnapActive = true;
            ChangeHelperText(s); 
            currentRoutine = ShakingRoutine();
            StartCoroutine(currentRoutine);
            snapBtn.interactable = false;
        }
        else
        {
            NotEnoughJewels();
        }
    }
    public void SnapUsed()
    {
        jm.JewelSpent(SnapCost);
        SnapDismissed();
    }

    public void SnapDismissed()
    {
        SnapActive = false;
        StartCoroutine(ButtonGCD(snapBtn));
        ChangeHelperText(" ");
    }

    public bool ShufflerActivated(string s)
    {
        bool result = false;
        if(jm.jewelAmount >= ShuffleCost)
        {
            result = true;
            ChangeHelperText(s);
            currentRoutine = TempTextRoutine();
            StartCoroutine(currentRoutine);
            jm.JewelSpent(ShuffleCost);
            StartCoroutine(ButtonGCD(shuffleBtn));
        }
        else
        {
            NotEnoughJewels();
        }
        return result;
    }

    public void ChangeHelperText(string s)
    {
        textHelper.text = s;
    }

    private void NotEnoughJewels()
    {
        textHelper.text = notEnough + $" ({SnapCost - jm.jewelAmount})";
        currentRoutine = TempTextRoutine();
        StartCoroutine(currentRoutine);
    }

    IEnumerator ButtonGCD(Button btn)
    {
        btn.interactable = false;
        yield return new WaitForSeconds(GCD);
        btn.interactable = true;
    }

    IEnumerator TempTextRoutine()
    {
        yield return new WaitForSeconds(textFadeAnimationTime);
        ChangeHelperText(" ");
    }

    IEnumerator ShakingRoutine()
    {
        if(SnapActive)
            fAnim.ShakeAll();

        yield return new WaitForSeconds(1.5f);

        if(SnapActive)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = ShakingRoutine();
            StartCoroutine(currentRoutine);
        }
    }
}
