using System.Collections;
using Project.Scripts.Pool;
using UnityEngine;
using System.Collections.Generic;
using Project.Scripts.Level;

namespace Project.Scripts.Character.Attack
{
    public class ThrowItem : GameUnit
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy")) SimplePool.Despawn(this); 
            var character =  other.GetComponent<Character>();
           if (character != null)
           {
               switch (character)
               {
                   case Bot bot:
                       bot.OnHit();
                       break;
                   case Player player:
                       player.OnHit();
                       break;
               }
           }

          
        }
       
    }
}