using System;
using UnityEngine;

namespace VMC.Ingame.Energy
{
    public class Health : IEnergy
    {
        public float health;
        public float maxHealth;
        public event Action<float> OnHealthChange;
        public event Action OnDeath;
        public bool IsAlive => health > 0;


        public void Init(float startValue, float maxValue)
        {
            this.health = startValue;
            this.maxHealth = maxValue;
            health = Mathf.Clamp(health, 0, maxHealth);
            OnHealthChange?.Invoke(health);
        }

        public void Sub(float value)
        {
            health -= value;
            health = Mathf.Clamp(health, 0, maxHealth);
            OnHealthChange?.Invoke(health);
            if(health<=0)
            {
                OnDeath?.Invoke();
            }
        }
        public void Revive(float percent)
        {
            this.health = maxHealth * percent;
            health = Mathf.Clamp(health, 1, maxHealth);
            OnHealthChange?.Invoke(health);
        }

    }
}