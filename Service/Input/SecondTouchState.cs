using UnityEngine;

public class SecondTouchState : InputState
{
    public SecondTouchState(InputManager inputManager) : base(inputManager)
    {
    }

    public override void DetectStuff()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int lm = LayerMask.GetMask("Selectables");

            if(Physics.Raycast(ray, out hit, lm))
            {
                if(hit.collider.tag == "Selectable" || hit.collider.tag == "Player")
                {
                    if(_inputManager.figureCtrl.SelectFigure(hit.collider.gameObject))
                    {
                        _inputManager.SetState(new FirstTouchState(_inputManager));
                    }
                    // else
                    // {
                    //     _inputManager.SetState(new FirstTouchState(_inputManager));
                    // }
                }
                else if(hit.collider.tag == "Tile")
                {
                    _inputManager.figureCtrl.TileSelected(hit.collider.gameObject);
                }
            }
            else
            {
                _inputManager.figureCtrl.DeselectFigure();
                _inputManager.SetState(new FirstTouchState(_inputManager));
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
