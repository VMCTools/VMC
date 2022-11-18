using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using VMC.Ultilities;

namespace VMC.Ingame.Move
{
    public class MoveWithJoyStickPOV : MoveWithJoyStick
    {
        protected override void Joystick_OnDragAction(Vector2 delta)
        {
            direction = transform.forward * delta.y + transform.right * delta.x;
        }
        protected override void GetInputKeyboard()
        {
            if (isDragging) return;
            if (!getInputFromKeyboard) return;
            direction = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
            isGetingInputFromKeyboard = direction.x != 0 || direction.z != 0;
        }
    }
}