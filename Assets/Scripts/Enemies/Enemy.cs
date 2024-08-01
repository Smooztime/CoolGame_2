using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected static Transform playerTransform;

    [Header("-----Enemy Health Bar-----")]
    [SerializeField]
    HealthBar enemyHealthBar;
    [SerializeField]
    private string damageText;
    [SerializeField]
    private Transform damageTextPos;

    [Header("-----Enemy Stat-----")]
    [SerializeField]
    protected int enemyMaxHP;
    [SerializeField]
    protected int enemyDamage;
    [SerializeField]
    protected float EnemySpeed;
    [SerializeField]
    protected float distanceBetween;
    [SerializeField]
    protected string enemyWeapon;
    [SerializeField]
    protected bool _haveSpawner;

    [Header("-----VFX-----")]
    [SerializeField]
    protected string fxName;

    [Header("-----Item-----")]
    [SerializeField]
    private string itemName;

    protected int currentEnemyHP;
    private float distance;
    private Animator _anim;
    private bool _isAlive = true;
    
    

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

    public void DamageToEnemy(int damage)
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
            _isAlive = false;
            DropPotion();
            if (_haveSpawner == true)
            {
                EnemySpawner.Instance.EnemySubtract(1);
                EnemySpawner.Instance.SpawnEnemy();
            }
            else
            {
                return;
            }
        }
        enemyHealthBar.UpdateHealthBar(enemyMaxHP, currentEnemyHP);

        if (gameObject != null && currentEnemyHP > 0)
        {
            ShowDamageText(damage);
        }
    }

    private void ShowDamageText(int damage)
    {
        DamageText _damageText = (DamageText)PoolManager.Instance.Spawn(damageText);
        _damageText.transform.position = damageTextPos.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        _damageText.transform.rotation = damageTextPos.rotation;
        _damageText.GetComponentInChildren<TextMeshPro>().text = damage.ToString();
    }

    protected virtual IEnumerator EnemyDestroy()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    protected void EnemyChase()
    {
        distance = Vector2.Distance(transform.position, playerTransform.position);
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        if (_isAlive)
        {
            if (distance < distanceBetween)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, playerTransform.position, EnemySpeed * Time.fixedDeltaTime);
                _anim.SetFloat("Horizontal", direction.x);
                _anim.SetFloat("Vertical", direction.y);
            }
        }
        else
        {
            return;
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
        if(chance > 80)
        {
            Potion potion = (Potion)PoolManager.Instance.Spawn(itemName);
            potion.transform.position = transform.position;
            potion.transform.rotation = transform.rotation;
        }
    }
}
