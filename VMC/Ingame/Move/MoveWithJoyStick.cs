using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMC.Ingame.Move
{
    public class MoveWithJoyStick : MonoBehaviour
    {
        public CharacterController character;
        public float speed;

        private bool isMoving;
        private Vector3 direction;

        private void Awake()
        {
            Joystick.OnStartedDragAction += Joystick_OnStartedDragAction;
            Joystick.OnDragAction += Joystick_OnDragAction;
            Joystick.OnEndedDragAction += Joystick_OnEndedDragAction;
        }
        private void Joystick_OnStartedDragAction()
        {
            isMoving = true;
        }
        private void Joystick_OnDragAction(Vector2 delta)
        {
            direction.x = delta.x;
            direction.z = delta.y;
        }
        private void Joystick_OnEndedDragAction()
        {
            isMoving = false;
        }

        private void Update()
        {
            if (isMoving)
                character.Move(speed * Time.deltaTime * direction);
        }
    }
}