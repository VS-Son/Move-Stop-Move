using Project.Scripts.Pool;
using UnityEngine;

namespace Project.Scripts.Character.Attack
{
    public class ThrowItem : GameUnit
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy")) SimplePool.Despawn(this);
            other.GetComponent<Character>().OnHit();
        }
    }
}