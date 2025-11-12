using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : GameUnit
{
    private Rigidbody _rigidbody ;

    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SimplePool.Despawn(this);
        }
    }
}
