using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMC.Ingame.Move
{
    public class MoveToTarget : Movement
    {
        protected Transform target;

        public void InitData(Transform target)
        {
            this.target = target;
            isCompleted = false;
        }
        public override bool IsComplete()
        {
            return Vector3.Distance(transform.position, target.position) < stopRange;
        }

        public override void Move()
        {
            direction = (target.position - transform.position).normalized;
            base.Move();
        }
    }
}