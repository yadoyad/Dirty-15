using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapActivatedState : InputState
{
    private Vector2 firstPressPos;
    public SnapActivatedState(InputManager inputManager) : base(inputManager){}

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
                if(hit.collider.tag == "Selectable" || hit.collider.tag == "NotSelectable")
                {
                    _inputManager.figureCtrl.RemoveFigure(hit.collider.gameObject);
                    _inputManager.abilitiesCtrl.SnapUsed();
                    _inputManager.SetState(new IdleState(_inputManager));
                }
                else
                {
                    _inputManager.abilitiesCtrl.SnapDismissed();
                }
            }
            else
            {
                _inputManager.abilitiesCtrl.SnapDismissed();
            }
        }
        if(GameJobsCtrl.instance.isBusy)
        {
            _inputManager.abilitiesCtrl.SnapDismissed();
            _inputManager.SetState(new IdleState(_inputManager));
        }
        //Можно будет удалить как только добавлю связь аниматора UI с GameJobsCtrl
        if(UIWindowsCtrl.instance.windowOpened)
        {
            _inputManager.abilitiesCtrl.SnapDismissed();
            _inputManager.SetState(new IdleState(_inputManager));
        }
        if(!AbilitiesCtrl.instance.SnapActive)
        {
            _inputManager.abilitiesCtrl.SnapDismissed();
            _inputManager.SetState(new IdleState(_inputManager));
        }
    }
}
