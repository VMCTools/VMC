using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
    public class PoolExample : MonoBehaviour
    {
        public GameObject myEnemyPrefab;
        // Start is called before the first frame update
        void Start()
        {

            // Use Spawn() instead of Unity's Instantiate()
            Transform enemy = PoolManager.Pools["Enemies"].Spawn(myEnemyPrefab);

            // Use Despawn() instead of Unity's Destroy() to de-spawn an enemy for reuse
            PoolManager.Pools["Enemies"].Despawn(enemy);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}