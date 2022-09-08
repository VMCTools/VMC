using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VMC.Ultilities;

namespace VMC.Ingame.Move
{
    public class MoveWithJoyStick : MonoBehaviour
    {
        public CharacterController character;
        public float speed;

        public bool isMoving;
        private Vector3 direction;
        public float CurrentSpeed => direction.SetY(0).magnitude * speed;

        private void Awake()
        {
            Joystick.OnStartedDragAction += Joystick_OnStartedDragAction;
            Joystick.OnDragAction += Joystick_OnDragAction;
            Joystick.OnEndedDragAction += Joystick_OnEndedDragAction;
        }

        public void SetSpeed(float moveSpeed)
        {
            this.speed = moveSpeed;
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
            direction = Vector3.zero;
        }

        private void Update()
        {
            if (isMoving)
            {
                direction.y = character.isGrounded ? 0f : -0.5f;
                character.Move(speed * Time.deltaTime * direction);
                transform.LookAt(transform.position + direction.SetY(0));
            }
        }
    }
}