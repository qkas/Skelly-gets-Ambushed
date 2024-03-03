using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject melee;
    public GameObject dagger;

    public float attackCooldown = 0.1f;
    private float attackTimer = 0f;

    public float dashStrength = 200f;
    public float dashCooldown = 1.0f;
    private float dashTimer = 0f;

    private bool stunned = false;
    public float stunTime = 0.3f;

    public float moveSpeed = 30f;
    public float rotationSpeed = 10f;
    private Vector3 moveInput;
    private Vector3 mousePosition;
    Quaternion lookRotation;

    public float damageDone = 0f;
    public float damageNeededForDagger = 200f;
    private float health, maxHealth = 100;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        // dash (movement)
        dash();

        // melee attack
        meleeAttack(); // if 'mouse1' or 'K' pressed

        // ranged attack
        throwDagger(); // if 'mouse2' or 'L' pressed
    }

    void FixedUpdate()
    {
        // get movement input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.z = Input.GetAxisRaw("Vertical");

        // convert mouse position to world coordinates
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        if (plane.Raycast(ray, out float distance))
        {
            mousePosition = ray.GetPoint(distance);
        }

        // calculate rotation towards cursor
        Vector3 lookDirection = mousePosition - transform.position;
        lookDirection.y = 0f; // lock y axis
        lookRotation = Quaternion.LookRotation(lookDirection);

        // rotate the player smoothly towards cursor
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookRotation, rotationSpeed * Time.fixedDeltaTime));

        // move the player
        if (!stunned)
        {
            rb.velocity = moveInput.normalized * moveSpeed * Time.deltaTime;
        }
    }

    IEnumerator StunTimer(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        stunned = false;
    }

    public void TakeDamage(float damage, float knockStrength, GameObject enemy)
    {
        // take damage
        health -= damage;

        // get knocked back
        Vector3 awayFromEnemy = (transform.position - enemy.gameObject.transform.position).normalized;
        rb.AddForce(awayFromEnemy * knockStrength, ForceMode.Impulse);

        // die if health 0
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void dash()
    {
        dashTimer -= Time.deltaTime;

        // dash towards movement input direction if spacebar/shift is pressed and dash is out of cooldown
        if (dashTimer <= 0 && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift)))
        {
            // prevent general movement while dashing
            stunned = true;
            StartCoroutine(StunTimer(stunTime));
            rb.AddForce(moveInput.normalized * dashStrength, ForceMode.Impulse);

            dashTimer = dashCooldown;
        }
    }

    private void meleeAttack()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0 && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.K)))
        {
            // instantiate attack infront of player
            Vector3 spawnPos = transform.position + transform.forward;
            Instantiate(melee, spawnPos, transform.rotation);

            attackTimer = attackCooldown;
        }
    }

    private void throwDagger()
    {
        if (damageDone >= damageNeededForDagger && (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.L)))
        {
            // instantiate attack infront of player
            Vector3 spawnPos = transform.position + transform.forward;
            Instantiate(dagger, spawnPos, lookRotation);

            // reduce dagger progress
            damageDone -= damageNeededForDagger;
            damageDone *= 0.5f;
        }
    }
}