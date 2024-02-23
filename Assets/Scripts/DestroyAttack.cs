using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DestroyAttack : MonoBehaviour
{
    public float upTime = 0.2f;
    private float timer = 0;

    void Start()
    {
        
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= upTime)
        {
            Destroy(gameObject);
            timer = 0;
        }
    }
}
