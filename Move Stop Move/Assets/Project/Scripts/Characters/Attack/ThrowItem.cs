using Project.Scripts.Pool;
using UnityEngine;

namespace Project.Scripts.Characters.Attack
{
    public class ThrowItem : GameUnit
    {
        
        private void OnTriggerEnter(Collider other)
        {
            var character = other.GetComponent<Character>();
            
            if (other.CompareTag("Neutral"))
            {
                SimplePool.Despawn(this);
                if (character != null)
                    switch (character)
                    {
                        case Player player:
                            player.OnHit();
                            break;
                        case Bot bot:
                            bot.OnHit();
                            break;
                    }
            }
        }

       
    }
}