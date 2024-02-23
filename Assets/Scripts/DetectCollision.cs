using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public bool destroySelf = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (destroySelf)
            {
                Destroy(gameObject);
            }
            Destroy(other.gameObject);
        }
    }
}
