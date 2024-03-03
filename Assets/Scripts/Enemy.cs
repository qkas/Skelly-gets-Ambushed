using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 20f;
    private bool stunned = false;
    public Rigidbody rb;
    private GameObject target;
    private PlayerControl playerControlScript;

    public float rotationSpeed = 5f;

    public GameObject attackPrefab;
    public float attackRange = 1.2f;
    public float attackCooldown = 0.2f;

    // can't attack straight after spawning
    private float attackTimer = 1f;

    private float health, maxHealth = 100;

    void Start()
    {
        health = maxHealth;

        // find target (player) and player controller script
        target = GameObject.FindGameObjectWithTag("Player");
        playerControlScript = target.GetComponent<PlayerControl>();
    }

    private void Update()
    {
        // melee attack
        attackTimer -= Time.deltaTime;
        if (target)
        {
            // if player is in range and cooldown is ready
            if ((rb.transform.position - target.transform.position).magnitude < attackRange && attackTimer <= 0)
            {
                // instantiate attack infront of self
                Vector3 spawnPos = transform.position + transform.forward;
                Instantiate(attackPrefab, spawnPos, transform.rotation);
                attackTimer = attackCooldown;
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
                rb.velocity = moveDirection * moveSpeed * Time.deltaTime;
            }
        }
    }

    public void TakeDamage(float damage, float knockStrength, float stunTime)
    {
        // take damage and update player control damage done tracker
        health -= damage;
        playerControlScript.damageDone += damage;

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
