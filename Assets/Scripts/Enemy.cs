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

    public GameObject attackPrefab;
    public float attackRange = 0.2f;
    public float meleeAttackCooldown = 0.2f;
    private float meleeAttackTimer = 0;

    private float health, maxHealth = 100;

    void Start()
    {
        // set health to max
        health = maxHealth;

        // find target (player)
        target = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        // melee attack
        meleeAttackTimer += Time.deltaTime;
        if ((rb.transform.position - target.transform.position).magnitude < attackRange+1 && meleeAttackTimer >= meleeAttackCooldown)
        {
            Vector3 spawnPos = transform.position + transform.forward;
            Instantiate(attackPrefab, spawnPos, transform.rotation);
            meleeAttackTimer = 0;
        }
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
