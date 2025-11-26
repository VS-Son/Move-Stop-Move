using UnityEngine;

namespace Project.Scripts.Pool
{
    public class PoolController : MonoBehaviour
    {
        [Header("Pool")] public PoolAmount[] Pool;

        [Header("Particle")] public ParticleAmount[] Particle;


        public void Awake()
        {
            foreach (var t in Particle)
                ParticlePool.Preload(t.prefab, t.amount, t.root);

            foreach (var pool in Pool)
                SimplePool.Preload(pool.prefab, pool.amount, pool.root, pool.collect, pool.clamp);
        }
    }
}