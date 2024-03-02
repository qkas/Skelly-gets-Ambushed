using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerAttack : MonoBehaviour
{
    public float damage = 100;
    public float knockStrength = 1000;
    public float stunTime = 0.3f;


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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy)
            {
                Destroy(gameObject);
                enemy.TakeDamage(damage, knockStrength, stunTime);
            }
        }
    }
}