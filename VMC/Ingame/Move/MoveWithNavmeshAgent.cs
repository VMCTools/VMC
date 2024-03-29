using System;
using System.Collections;
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
        private bool isSetTarget = false;
        public Vector3 targetPos;
        public override Vector3 TargetPos => targetPos;

        public override float Speed => navmeshAgent.velocity.magnitude;
        public override bool IsComplete()
        {
            return false;
        }
        public override void SetSpeed(float speed)
        {
            base.SetSpeed(speed);
            navmeshAgent.speed = this.speed;
        }
        public override void SetPosition(Vector3 position)
        {
            navmeshAgent.Warp(position);
        }
        public override void MoveTo(Vector3 targetPosition)
        {
            navmeshAgent.isStopped = false;
            this.isSetTarget = true;
            this.targetPos = targetPosition;
            navmeshAgent.SetDestination(targetPosition);
            isMoving = true;
        }
        public override void Pause()
        {
            if (navmeshAgent.enabled)
                navmeshAgent.isStopped = true;
            base.Pause();
        }
        public override void Resume()
        {
            navmeshAgent.isStopped = false;
            if (isSetTarget)
                MoveTo(targetPos);
            base.Resume();
        }
        public override void Stop()
        {
            navmeshAgent.isStopped = true;
            isSetTarget = false;
            base.Stop();
        }

        public bool DestinationOnNavMesh(Vector3 checkPos)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(checkPos, out hit, 1f, NavMesh.AllAreas))
                return true;
            return false;
        }
    }
}
