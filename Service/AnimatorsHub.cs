using UnityEngine;

//Чтобы не править код в других местах, все будут проверять готовность анимаций здесь
public class AnimatorsHub : MonoBehaviour
{
    public static AnimatorsHub instance;
    public bool allReady {get; private set;}
    FigureScriptedAnimations fAnim;
    
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

    private void OnEnable() 
    {
        fAnim = FindObjectOfType<FigureScriptedAnimations>();
    }

    void Update()
    {
        ReadyCheck();
    }

    void ReadyCheck()
    {
        //По мере добавления аниматоров, нужно расширять этот if
        if(!fAnim.movementInProgress)
            allReady = true;
        else
            allReady = false;
    }
}
