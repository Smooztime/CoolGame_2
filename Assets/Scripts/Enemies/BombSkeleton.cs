using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BombSkeleton : Enemy
{
    [SerializeField]
    private string bombFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        EnemyChase();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController player))
        {
            FX _fx = (FX)PoolManager.Instance.Spawn(bombFX);
            _fx.transform.position = transform.position;
            _fx.transform.rotation = transform.rotation;
            Destroy(gameObject);
            player.DamageToPlayer(enemyDamage);
            EnemySpawner.Instance.EnemySubtract(1);
            EnemySpawner.Instance.SpawnEnemy();
        }
    }
}
