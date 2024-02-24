using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAttack : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 10;
    private float xRange = 20;
    private float zRange = 20;

    void Update()
    {
        if (transform.position.x < -xRange || transform.position.x > xRange ||
            transform.position.z < -zRange || transform.position.z > zRange)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;
    }
}
