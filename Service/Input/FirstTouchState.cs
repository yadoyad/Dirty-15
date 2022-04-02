using UnityEngine;

public class FirstTouchState : InputState
{
    private Vector2 firstPressPos;
    public FirstTouchState(InputManager inputManager) : base (inputManager) {}
    public override void DetectStuff()
    {
        if(Input.GetMouseButtonDown(0))
        {
            firstPressPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int lm = LayerMask.GetMask("Selectables");

            if(Physics.Raycast(ray, out hit, lm))
            {
                if(hit.collider.tag == "Selectable" || hit.collider.tag == "Player")
                {
                    int moves = _inputManager.figureCtrl.SelectFigure(hit.collider.gameObject, true);
                    if(moves > 1)
                        _inputManager.SetState(new SecondTouchState(_inputManager));
                    // else if(moves == 1)
                    // {
                    //     _inputManager.figureCtrl.TileSelected();
                    // }
                }
            }
        }
        if(GameJobsCtrl.instance.isBusy)
        {
            _inputManager.SetState(new IdleState(_inputManager));
        }
        //Можно будет удалить как только добавлю связь аниматора UI с GameJobsCtrl
        if(UIWindowsCtrl.instance.windowOpened)
        {
            _inputManager.SetState(new IdleState(_inputManager));
        }
        if(AbilitiesCtrl.instance.SnapActive)
        {
            _inputManager.SetState(new SnapActivatedState(_inputManager));
        }
    }
}
