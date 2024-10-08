using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : PoolObject
{
    [SerializeField]
    private int boneDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerController damage))
        {
            damage.DamageToPlayer(boneDamage);
        }
        DeSpawn();
    }
}
