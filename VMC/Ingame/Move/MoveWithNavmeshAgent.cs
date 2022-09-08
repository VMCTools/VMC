using System;
using UnityEngine;
using UnityEngine.AI;

namespace VMC.Ingame.Move
{
    public class MoveWithNavmeshAgent : Movement
    {
        [SerializeField] private NavMeshAgent navmeshAgent;
        public override bool IsComplete()
        {
            return true;
        }
        public override void SetSpeed(float speed)
        {
            base.SetSpeed(speed);
            navmeshAgent.speed = this.speed;
        }
        public void MoveTo(Vector3 targetPosition, float stopDistance)
        {
            navmeshAgent.stoppingDistance = stopDistance;
            navmeshAgent.destination = targetPosition;
            navmeshAgent.isStopped = false;
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
        
        public void MoveTo(Vector3 targetPosition, Action complete)
        {
            navmeshAgent.stoppingDistance = 0.1f;
            navmeshAgent.SetDestination(targetPosition);
            navmeshAgent.isStopped = false;
        }
    }
}
