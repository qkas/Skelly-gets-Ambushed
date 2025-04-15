using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    private bool isStunned = false;
    public Rigidbody rb;
    private GameObject target;
    private PlayerControl playerControlScript;

    public float rotationSpeed = 5f;

    public AudioSource audioSource;
    public AudioClip getHitSound;
    public AudioClip deathSound;

    public GameObject attackPrefab;
    public float attackRange = 1.2f;
    public float attackCooldown = 0.2f;

    // can't attack straight after spawning
    private float attackTimer = 1f;

    private float health, maxHealth = 100;

    public bool isDead = false;
    public float dieTime = 1f;

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
        if (target && !isStunned && !isDead)
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
        if (target && !isStunned && !isDead)
        {
            // get direction of player
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;

            // set rotation of enemy
            Quaternion lookDirection = Quaternion.LookRotation(moveDirection);

            // rotate and move enemy towards player 
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookDirection, rotationSpeed * Time.fixedDeltaTime));

            // move enemy towards player
            rb.velocity = moveDirection * moveSpeed;
        }
        if (isDead)
        {
            rb.velocity = Vector3.zero;
        }
    }

    public void TakeDamage(float damage, float knockStrength, float stunTime)
    {
        // play audio
        audioSource.PlayOneShot(getHitSound, 0.8f);

        // take damage and update player control damage done tracker
        health -= damage;
        playerControlScript.AddFury(damage);

        // stun player
        isStunned = true;
        StartCoroutine(StunTimer(stunTime));

        // get knocked back
        Vector3 awayFromTarget = (transform.position - target.gameObject.transform.position).normalized;
        rb.AddForce(awayFromTarget * knockStrength, ForceMode.Impulse);

        // die if health 0
        if (health <= 0)
        {
            // play death sound
            audioSource.PlayOneShot(deathSound, 0.8f);

            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        reduceColorAlpha();
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    private void reduceColorAlpha()
    {
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        Color color = mr.material.color;
        color.a = 0.25f;
        mr.material.SetColor("_Color", color);
    }

    IEnumerator StunTimer(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
    }
}
