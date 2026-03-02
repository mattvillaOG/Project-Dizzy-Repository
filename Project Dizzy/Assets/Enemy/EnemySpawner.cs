using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Types")]
    [SerializeField] private GameObject[] enemyPrefabs;   // Multiple enemy types

    [Header("Pooling Per Type")]
    [SerializeField] private int poolSizePerType = 5;

    [Header("Spawn Timing (seconds)")]
    [SerializeField] private float spawnInterval = 2f;

    [Header("Spawn Options")]
    [SerializeField] private bool spawnOnStart = true;

    private GameObject[][] pools;   // 2D array: one pool per prefab type
    private float timer;

    private void Awake()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("EnemySpawner: No enemy prefabs assigned.");
            enabled = false;
            return;
        }

        poolSizePerType = Mathf.Max(1, poolSizePerType);

        // Create pools
        pools = new GameObject[enemyPrefabs.Length][];

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (enemyPrefabs[i] == null)
            {
                Debug.LogError($"EnemySpawner: Enemy prefab at index {i} is null.");
                continue;
            }

            pools[i] = new GameObject[poolSizePerType];

            for (int j = 0; j < poolSizePerType; j++)
            {
                GameObject enemy = Instantiate(enemyPrefabs[i]);
                enemy.SetActive(false);
                pools[i][j] = enemy;
            }
        }
    }

    private void Start()
    {
        timer = 0f;

        if (spawnOnStart)
        {
            TrySpawn();
            timer = 0f;
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
        int randomType = Random.Range(0, enemyPrefabs.Length);

        GameObject enemy = GetInactiveEnemyFromPool(randomType);
        if (enemy == null)
        {
            // All enemies of this type are active
            return;
        }

        enemy.transform.position = transform.position;
        enemy.transform.rotation = transform.rotation;

        enemy.SetActive(true);
    }

    private GameObject GetInactiveEnemyFromPool(int typeIndex)
    {
        for (int i = 0; i < pools[typeIndex].Length; i++)
        {
            if (!pools[typeIndex][i].activeInHierarchy)
                return pools[typeIndex][i];
        }
        return null;
    }

    public void SetSpawnInterval(float seconds)
    {
        spawnInterval = Mathf.Max(0f, seconds);
        timer = 0f;
    }
}