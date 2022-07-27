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
            if (!CanMove()) return;
            transform.position += (Vector3)(speed * Time.deltaTime * direction);
        }
        public bool CanMove()
        {
            if (!isMoving) return false;
            if (isPausing) return false;
            if (isCompleted) return false;
            return true;
        }
        public virtual void InitData()
        {

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