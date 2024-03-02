using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public float damage = 35;
    public float knockStrength = 1000;
    public float stunTime = 0.3f;

    private void OnTriggerEnter(Collider other)
    {
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
