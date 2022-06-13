using UnityEngine;
namespace VMC.Ingame.Move
{
    public abstract class Movement : MonoBehaviour, IMove
    {
        protected float stopRange;
        protected Vector3 direction;
        protected float speed;

        protected bool isMoving;
        protected bool isPausing;
        protected bool isCompleted;

        public virtual void Move()
        {
            transform.position += (Vector3)(speed * Time.deltaTime * direction);
        }
        public virtual void InitData()
        {

        }
        protected virtual void Update()
        {
            if (!isMoving) return;
            if (isPausing) return;
            if (isCompleted) return;
            Move();
        }
        public virtual void Pause()
        {
            isPausing = true;
        }

        public virtual void Resume()
        {
            isPausing = false;
        }

        public virtual void Stop()
        {
            isMoving = false;
        }

        public abstract bool IsComplete();
    }
}