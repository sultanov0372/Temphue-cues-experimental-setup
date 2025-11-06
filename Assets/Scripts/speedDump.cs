using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedDump : MonoBehaviour
{
    public float maxSpeed = 0.5f;
    private Rigidbody rb;

    void Start() => rb = GetComponent<Rigidbody>();

    void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;
    }

}
