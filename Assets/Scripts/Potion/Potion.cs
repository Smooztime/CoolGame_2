using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : PoolObject
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent(out PlayerController heal))
        {
            heal.Heal(3);
            DeSpawn();
        }
    }
}
