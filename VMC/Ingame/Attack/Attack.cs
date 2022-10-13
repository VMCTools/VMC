using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMC.Ingame.Attack
{
    public class Attack : MonoBehaviour
    {
        public float damage;
        public float atkTime;
        public float dealDamageTime;

        public bool isAttacking = false;
        public event Action OnStartAttack;
        public event Action OnEndedAttack;
        public event Action OnDealDamage;
        public event Action OnCancelAttack;

        private float countTimeAtk;
        private bool isDealDamage;
        public void Init(float damage, float atkTime, float dealDamageTime)
        {
            this.damage = damage;
            this.atkTime = atkTime;
            this.dealDamageTime = dealDamageTime;
        }
        public void StartAttack()
        {
            isAttacking = true;
            countTimeAtk = 0f;
            isDealDamage = false;
            OnStartAttack?.Invoke();
        }
        public void EndAttack()
        {
            isAttacking = false;
            OnEndedAttack?.Invoke();
        }
        public void ReleaseDamage()
        {
            isDealDamage = true;
            OnDealDamage?.Invoke();
        }
        public void Cancel()
        {
            isAttacking = false;
            OnCancelAttack?.Invoke();
        }
        private void Update()
        {
            if (isAttacking)
            {
                countTimeAtk += Time.deltaTime;
                if (!isDealDamage)
                {
                    if (countTimeAtk >= dealDamageTime)
                    {
                        ReleaseDamage();
                    }
                }
                if (countTimeAtk >= atkTime)
                {
                    EndAttack();
                }
            }
        }
    }
}