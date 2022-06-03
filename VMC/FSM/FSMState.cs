using System;
using UnityEngine;

namespace VMC.FSM
{
    [Serializable]
    public class FSMState
    {
        public virtual void OnEnter()
        {

        }
        public virtual void OnEnter(object data)
        {

        }
        public virtual void OnUpdate()
        {

        }
        public virtual void OnLateUpdate()
        {

        }
        public virtual void OnFixedUpdate()
        {

        }

        public virtual void OnExit()
        {

        }
        public virtual void OnEventMiddleAnimation()
        {

        }
        public virtual void OnEventEndAnimation()
        {

        }

        public virtual void OnDestroy()
        {

        }
        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
        }
        public virtual void OnCollisionStay2D(Collision2D collision)
        {

        }
        public virtual void OnCollisionExit2D(Collision2D collision)
        {

        }

        public virtual void OnTriggerEnter2D(Collider2D collision)
        {

        }

        public virtual void OnTriggerStay2D(Collider2D collision)
        {

        }

        public virtual void OnTriggerExit2D(Collider2D collision)
        {

        }
    }
}