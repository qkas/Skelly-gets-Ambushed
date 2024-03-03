using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerAttack : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 20f;
    public float damage = 100f;
    public float knockStrength = 1000f;
    public float stunTime = 0.3f;
    public float destroyDelay = 2.0f;

    private void Start()
    {
        // destroy game object after delay
        Destroy(gameObject, destroyDelay);
    }

    void Update()
    {
        // fly forward
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        // damage enemy
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