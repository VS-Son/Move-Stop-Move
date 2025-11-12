using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public Rigidbody rigidbody
    {
        get => _rigidbody;
        set
        {
            _rigidbody = value;
            _rigidbody = GetComponent<Rigidbody>();
        } 
        
    }
    private Character Character => Character.Instance;
    
    private IEnumerator HideItemThrow()
    {
        if (Vector3.Distance(transform.position, Character.throwRange.position) > Character.rangeSize)
        {
            gameObject.SetActive(false);
        }
        yield return null;
    }
}
