using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damage = 25f;
    public float knockStrength = 500f;
    public float stunTime = 0.3f;
    public float destroyDelay = 0.05f;

    private void Start()
    {
        // destroy game object after delay
        Destroy(gameObject, destroyDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        // damage player
        if (other.CompareTag("Player"))
        {
            PlayerControl player = other.GetComponent<PlayerControl>();
            if (player)
            {
                player.TakeDamage(damage, knockStrength, other.gameObject);
            }
        }
    }
}
