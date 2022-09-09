using System;
using UnityEngine;
using UnityEngine.AI;

namespace VMC.Ingame.Move
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MoveWithNavmeshAgent : Movement
    {
        private NavMeshAgent _agent;
        private NavMeshAgent navmeshAgent
        {
            get
            {
                if (!_agent) _agent = GetComponent<NavMeshAgent>();
                return _agent;
            }
        }
        public override float Speed => navmeshAgent.velocity.magnitude;
        public override bool IsComplete()
        {
            return true;
        }
        public override void SetSpeed(float speed)
        {
            base.SetSpeed(speed);

            navmeshAgent.speed = this.speed;
        }
        public override void MoveTo(Vector3 targetPosition)
        {
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
    }
}
