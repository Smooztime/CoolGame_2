using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected static Transform playerTransform;

    [Header("-----Enemy Health Bar-----")]
    [SerializeField]
    HealthBar enemyHealthBar;

    [Header("-----Enemy Stat-----")]
    [SerializeField]
    protected float enemyMaxHP;
    [SerializeField]
    protected float enemyDamage;
    [SerializeField]
    protected float EnemySpeed;
    [SerializeField]
    protected float distanceBetween;
    [SerializeField]
    protected string enemyWeapon;

    [Header("-----VFX-----")]
    [SerializeField]
    protected string fxName;

    [Header("-----Item-----")]
    [SerializeField]
    private string itemName;

    protected float currentEnemyHP;
    private float distance;
    private Animator _anim;
    

    private void Awake()
    {
        currentEnemyHP = enemyMaxHP;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageToEnemy(float damage)
    {
        currentEnemyHP -= damage;
        Debug.Log(currentEnemyHP);
        if (currentEnemyHP <= 0)
        {
            FX _fx = (FX)PoolManager.Instance.Spawn(fxName);
            _fx.transform.position = transform.position;
            _fx.transform.rotation = transform.rotation;
            gameObject.GetComponent<Collider2D>().enabled = false;
            SoundManager.PlaySound(SoundType.EnemyDie);
            StartCoroutine(EnemyDestroy());
            DropPotion();
            EnemySpawner.Instance.EnemySubtract(1);
            EnemySpawner.Instance.SpawnEnemy();
        }
        enemyHealthBar.UpdateHealthBar(enemyMaxHP, currentEnemyHP);
    }

    private IEnumerator EnemyDestroy()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

    protected void EnemyChase()
    {
        distance = Vector2.Distance(transform.position, playerTransform.position);
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        if (distance < distanceBetween)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, playerTransform.position, EnemySpeed * Time.fixedDeltaTime);
            _anim.SetFloat("Horizontal", direction.x);
            _anim.SetFloat("Vertical", direction.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController damage))
        {
            damage.DamageToPlayer(enemyDamage);
        }
    }

    private void DropPotion()
    {
        int chance = Random.Range(0, 101);
        if(chance > 70)
        {
            Potion potion = (Potion)PoolManager.Instance.Spawn(itemName);
            potion.transform.position = transform.position;
            potion.transform.rotation = transform.rotation;
        }
    }
}
