using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace VMC.Ingame.Move
{
    public class MoveWithNavmeshAgent : Movement
    {
        [SerializeField] private NavMeshAgent navmeshAgent;

        public override bool IsComplete()
        {
            throw new System.NotImplementedException();
        }
        private void Start()
        {
            navmeshAgent.speed = this.speed;
        }
        public override void SetSpeed(float speed)
        {
            base.SetSpeed(speed);
            navmeshAgent.speed = this.speed;
        }
        public void MoveTo(Vector3 targetPosition, float stopDistance)
        {
            //this.targetPosition = targetPosition;
            //direction = (targetPosition - transform.position).normalized;
            //isCompleted = false;
            navmeshAgent.stoppingDistance = stopDistance;
            //navmeshAgent.SetDestination(targetPosition);
            navmeshAgent.Move(targetPosition);
        }
        public override void Pause()
        {
            navmeshAgent.isStopped = true;
            base.Pause();
        }
        public override void Resume()
        {
            navmeshAgent.isStopped = false;
            base.Resume();
        }
        public override void Stop()
        {
            navmeshAgent.isStopped = true;
            base.Stop();
        }

    }
}
