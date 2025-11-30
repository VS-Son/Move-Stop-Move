using Project.Scripts.Pool;
using UnityEngine;

namespace Project.Scripts.Characters
{
    public class GameUnit : MonoBehaviour
    {
        private Transform _tf;
        public Transform Tf
        {
            get
            {
                _tf = _tf ? _tf : gameObject.transform;
                return _tf;
            }
        }

        public PoolType poolType;

   

    }
}