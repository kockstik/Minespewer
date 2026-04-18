using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    private Vector3 moveDir = Vector3.zero;
    private Rigidbody rb;

    //[SerializeField] private float mapSize = 300f;
    //[SerializeField] private float playerSize = 8.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //mapSize -= playerSize / 2;
    }

    public void Move(Vector3 direction)
    {
        moveDir = direction.normalized;
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveDir * speed);

        /*
        if (transform.position.x > mapSize)
        {
            rb.linearVelocity = new Vector3(Mathf.Clamp(rb.linearVelocity.x, 0, -float.MinValue), 0, rb.linearVelocity.z);
        }
        else if (transform.position.x < -mapSize)
        {
            rb.linearVelocity = new Vector3(Mathf.Clamp(rb.linearVelocity.x, 0, float.MaxValue), 0, rb.linearVelocity.z);
        }

        if (transform.position.z > mapSize)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, Mathf.Clamp(rb.linearVelocity.z, 0, -float.MinValue));
        }
        else if (transform.position.z < -mapSize)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, Mathf.Clamp(rb.linearVelocity.z, 0, float.MaxValue));
        }
        */
    }
}
