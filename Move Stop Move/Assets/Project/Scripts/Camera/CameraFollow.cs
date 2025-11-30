using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Character;
using Project.Scripts.Characters;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float speed;

    private void Start()
    {
        target = FindObjectOfType<Player>().transform;
    }
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.fixedDeltaTime * speed);
        transform.rotation = Quaternion.Euler(rotation);
    }
}
