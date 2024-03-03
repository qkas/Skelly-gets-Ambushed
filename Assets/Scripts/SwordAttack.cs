using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public float damage = 35f;
    public float knockStrength = 1000f;
    public float stunTime = 0.3f;
    public float destroyDelay = 0.05f;

    private void Start()
    {
        // destroy game object after delay
        Destroy(gameObject, destroyDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        // damage enemy
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.TakeDamage(damage, knockStrength, stunTime);
            }
        }
    }
}
