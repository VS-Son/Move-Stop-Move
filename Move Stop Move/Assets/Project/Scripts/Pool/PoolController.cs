using UnityEngine;

namespace Project.Scripts.Pool
{
    public class PoolController : MonoBehaviour
    {
        [Header("Pool")]
        public PoolAmount[] Pool;

        [Header("Particle")]
        public ParticleAmount[] Particle;


        public void Awake()
        {
            for (int i = 0; i < Particle.Length; i++)
            {
                ParticlePool.Preload(Particle[i].prefab, Particle[i].amount, Particle[i].root);
            }

            foreach (var pool in Pool)
            {
                SimplePool.Preload(pool.prefab, pool.amount, pool.root, pool.collect, pool.clamp);
            }
        }
    }
}
