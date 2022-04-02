using UnityEngine;
using TMPro;

public class UIMovesCounter : MonoBehaviour
{
    public TextMeshProUGUI counter;
    private MovesCtrl movesCtrl;

    private void OnEnable() 
    {
        movesCtrl = FindObjectOfType<MovesCtrl>();
    }

    public void UpdateCounterText()
    {
        int result = movesCtrl.totalMoves - movesCtrl.movesCount;
        counter.text = result.ToString();
    }

    public GameObject GetMyObj() => gameObject;
}
