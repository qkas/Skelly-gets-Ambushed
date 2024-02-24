using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject meleeAttackPrefab;
    public GameObject rangedAttackPrefab;
    public Rigidbody rb;

    public float meleeAttackCooldown = .1f;
    private float meleeAttackTimer = 0;
    public float rangedAttackCooldown = 1.0f;
    private float rangedAttackTimer = 0;

    public float moveSpeed = 6;
    public float rotationSpeed = 10f;
    private Vector3 moveInput;
    private Vector3 mousePosition;
    Quaternion lookRotation;

    private float health, maxHealth = 100;

    void Start()
    {
        // set health to max
        health = maxHealth;

        // attacks ready at start
        meleeAttackTimer = meleeAttackCooldown;
        rangedAttackTimer = rangedAttackCooldown;
    }

    private void Update()
    {
        // melee attack
        meleeAttackTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0) && meleeAttackTimer >= meleeAttackCooldown)
        {
            Vector3 spawnPos = transform.position + transform.forward;
            Instantiate(meleeAttackPrefab, spawnPos, transform.rotation);
            meleeAttackTimer = 0;
        }

        // ranged attack
        rangedAttackTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse1) && rangedAttackTimer >= rangedAttackCooldown)
        {
            Vector3 spawnPos = transform.position + transform.forward;
            Instantiate(rangedAttackPrefab, spawnPos, lookRotation);
            rangedAttackTimer = 0;
        }
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
        rb.velocity = moveInput.normalized * moveSpeed;
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