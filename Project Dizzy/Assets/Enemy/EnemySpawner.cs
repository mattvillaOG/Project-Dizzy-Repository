using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Pooling")]
    [SerializeField] private int poolSize = 10;

    [Header("Spawn Timing (seconds)")]
    [SerializeField] private float spawnInterval = 2f;

    [Header("Spawn Options")]
    [SerializeField] private bool spawnOnStart = true;

    private GameObject[] pool;
    private float timer;

    private void Awake()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemySpawner: enemyPrefab is not assigned.");
            enabled = false;
            return;
        }

        poolSize = Mathf.Max(1, poolSize);

        pool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            pool[i] = enemy;
        }
    }

    private void Start()
    {
        timer = 0f;

        if (spawnOnStart)
        {
            TrySpawn();
            timer = 0f; // ensure we wait a full interval before the next spawn
        }
    }

    private void Update()
    {
        if (spawnInterval <= 0f) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            TrySpawn();
        }
    }

    private void TrySpawn()
    {
        GameObject enemy = GetInactiveEnemyFromPool();
        if (enemy == null)
        {
            // Pool is full: all enemies are active, so we do nothing this tick.
            return;
        }

        enemy.transform.position = transform.position;
        enemy.transform.rotation = transform.rotation;

        // Optional: reset enemy state here if needed
        // enemy.GetComponent<GhostScript>()?.ResetEnemy();

        enemy.SetActive(true);
    }

    private GameObject GetInactiveEnemyFromPool()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (!pool[i].activeInHierarchy)
                return pool[i];
        }
        return null;
    }

    // Optional helper if you want to change the interval at runtime
    public void SetSpawnInterval(float seconds)
    {
        spawnInterval = Mathf.Max(0f, seconds);
        timer = 0f;
    }
}