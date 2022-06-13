using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMC.Ingame.Move
{
    public class MoveToPosition : Movement
    {
        public Vector3 targetPosition;
        public void InitData(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
            direction = (targetPosition - transform.position).normalized;
            isCompleted = false;
        }
        public override void Move()
        {
            base.Move();
            if (IsComplete()) isCompleted = true;
        }
        public override bool IsComplete()
        {
            return Vector3.Distance(transform.position, targetPosition) < stopRange;
        }
    }
}