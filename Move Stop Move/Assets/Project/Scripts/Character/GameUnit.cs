using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Pool;
using UnityEngine;

public class GameUnit : MonoBehaviour
{
    private Transform tf;
    public Transform TF
    {
        get
        {
            tf = tf ?? gameObject.transform;
            return tf;
        }
    }

    public PoolType poolType;

   

}