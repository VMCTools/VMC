using UnityEngine;

namespace VMC.Ingame.Move
{
    public class MoveToPath : Movement
    {
        protected Vector3[] path;

        private int curIndex;
        public void InitData(Vector3[] path)
        {
            this.path = path;
            isCompleted = false;
            curIndex = 0;
        }

        public override bool IsComplete()
        {
            return isCompleted;
        }

        public override void Move()
        {
            if (!CanMove()) return;
            if (transform.position != path[curIndex])
            {
                Vector3 pos = Vector3.MoveTowards(transform.position, path[curIndex], speed * Time.deltaTime);
                transform.position = pos;
            }
            else
            {
                curIndex++;
                if (curIndex >= path.Length)
                {
                    isCompleted = true;
                }
            }
        }
    }
}