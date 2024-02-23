using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Rigidbody rb;
    public GameObject target;
    public float rotationSpeed = 5f;
    private Vector3 moveDirection;

    private float health, maxHealth = 100;

    void Start()
    {
        // set health to max
        health = maxHealth;

        // find target (player)
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (target)
        {
            // get direction of player
            moveDirection = (target.transform.position - transform.position).normalized;

            // set rotation of enemy
            Quaternion lookDirection = Quaternion.LookRotation(moveDirection);

            // Move and rotate enemy 
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookDirection, rotationSpeed * Time.fixedDeltaTime));
            rb.velocity = moveDirection * moveSpeed;
        }
    }
    public void TakeDamage(float damage)
    {
        // take damage
        health -= damage;

        // die if health 0
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
