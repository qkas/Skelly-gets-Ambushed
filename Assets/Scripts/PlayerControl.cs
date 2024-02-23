using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject attackPrefab;
    public Rigidbody rb;
    public float attackCooldown = .5f;
    private float attackTimer = 0;
    public float moveSpeed = 6;
    public float rotationSpeed = 10f;
    private Vector3 moveInput;

    void Start()
    {
        // first attack ready from start
        attackTimer = attackCooldown;
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0) && attackTimer >= attackCooldown)
        {
            Vector3 spawnPos = transform.position + transform.forward;
            Instantiate(attackPrefab, spawnPos, transform.rotation);
            attackTimer = 0;
        }
    }

    void FixedUpdate()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.z = Input.GetAxisRaw("Vertical");

        if (moveInput.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput.normalized);
            Quaternion newRotation = Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(newRotation);
        }

        rb.velocity = moveInput.normalized * moveSpeed;
    }
}