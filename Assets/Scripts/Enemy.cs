using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    private bool stunned = false;
    public Rigidbody rb;
    public GameObject target;
    public float rotationSpeed = 5f;

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
        if (target)
        {
            if ((rb.transform.position - target.transform.position).magnitude < attackRange+1 && meleeAttackTimer >= meleeAttackCooldown)
            {
                Vector3 spawnPos = transform.position + transform.forward;
                Instantiate(attackPrefab, spawnPos, transform.rotation);
                meleeAttackTimer = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        if (target)
        {
            // get direction of player
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;

            // set rotation of enemy
            Quaternion lookDirection = Quaternion.LookRotation(moveDirection);

            // rotate and move enemy towards player 
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookDirection, rotationSpeed * Time.fixedDeltaTime));
            if (!stunned)
            {
                rb.velocity = moveDirection * moveSpeed;
            }
        }
    }

    public void TakeDamage(float damage, float knockStrength, float stunTime)
    {
        // take damage
        health -= damage;

        // stun player
        stunned = true;
        StartCoroutine(StunTimer(stunTime));

        // get knocked back
        Vector3 awayFromTarget = (transform.position - target.gameObject.transform.position).normalized;
        rb.AddForce(awayFromTarget * knockStrength, ForceMode.Impulse);

        // die if health 0
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator StunTimer(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        stunned = false;
    }
}
