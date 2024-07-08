using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("-----Spawner-----")]
    [SerializeField]
    private HealthBar spawnerHealthBar;
    [SerializeField]
    protected float spawnerMaxHP;
    [SerializeField]
    private GameObject spawnerSprite;
    [SerializeField]
    private Transform spawnBox;
    [SerializeField]
    private string fxName;

    [Header("-----Enemy Spawn-----")]
    [SerializeField]
    private GameObject[] enemy;
    [SerializeField]
    private int enemyLimit;
    [SerializeField]
    private LayerMask layerCheck;

    private float currentSpawnerHP;
    private int enemyCount = 0;
    private bool _isSpawnerActive;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentSpawnerHP = spawnerMaxHP;
        StartCoroutine(SpawnTime());
        _isSpawnerActive = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DamageToSpawner(float damage)
    {
        currentSpawnerHP -= damage;
        Debug.Log(currentSpawnerHP);
        if (currentSpawnerHP <= 0)
        {
            Instantiate(spawnerSprite, transform.position, Quaternion.identity);
            FX _fx = (FX)PoolManager.Instance.Spawn(fxName);
            _fx.transform.position = transform.position;
            _fx.transform.rotation = transform.rotation;
            Destroy(gameObject);
            _isSpawnerActive = false;
        }
        spawnerHealthBar.UpdateHealthBar(spawnerMaxHP, currentSpawnerHP);
    }

    public void SpawnEnemy()
    {
        if(_isSpawnerActive)
        {
            if (!IsObtacle())
            {
                StartCoroutine(SpawnTime());
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    public void EnemySubtract(int value)
    {
        enemyCount -= value;
    }

    public IEnumerator SpawnTime()
    {
        while (enemyCount < enemyLimit)
        {
            enemyCount += 1;
            yield return new WaitForSeconds(0.1f);
            Vector3 spawnPoint = transform.localScale;
            Vector3 spawnPos = new Vector3(Random.value * spawnPoint.x, Random.value * spawnPoint.y);
            spawnPos = spawnBox.transform.TransformPoint(spawnPos - spawnPoint / 2);
            int randomEnemy = Random.Range(0, enemy.Length);
            Instantiate(enemy[randomEnemy], spawnPos, Quaternion.identity);
        }
    }

    private bool IsObtacle()
    {
        Vector2 origin = spawnBox.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 1f, layerCheck);
        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
