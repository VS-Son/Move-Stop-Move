using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

    }

    public void OnThrow(Vector3 direction, float speed)
    {
        _rigidbody.velocity = direction * speed;
    }
}
