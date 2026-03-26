using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour
{
    [Header("Prefabs to Spawn")]
    [SerializeField] private GameObject[] prefabsToSpawn;

    [Header("Spawn Timing")]
    [SerializeField] private float minRespawnTime = 1f;
    [SerializeField] private float maxRespawnTime = 3f;

    [Header("Selection Mode")]
    [SerializeField] private bool randomPrefab = true;

    private GameObject currentInstance;
    private BasicPowerUp currentPowerUp;
    private Coroutine respawnCoroutine;
    private int nextIndex = 0;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (!isActiveAndEnabled) return;

        if (prefabsToSpawn == null || prefabsToSpawn.Length == 0)
        {
            Debug.LogError("SpawnerScript: No prefabs assigned in prefabsToSpawn.");
            return;
        }

        // Pick which prefab to spawn
        GameObject prefab =
            randomPrefab ? prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)]
                         : prefabsToSpawn[nextIndex];

        if (!randomPrefab)
            nextIndex = (nextIndex + 1) % prefabsToSpawn.Length;

        currentInstance = Instantiate(prefab, transform.position, Quaternion.identity);

        currentPowerUp = currentInstance.GetComponent<BasicPowerUp>();
        if (currentPowerUp != null)
        {
            currentPowerUp.OnDisabled += HandleObjectDisabled;
        }
        else
        {
            Debug.LogError("SpawnerScript: Spawned prefab is missing BasicPowerUp component.");
        }
    }

    private void HandleObjectDisabled()
    {
        if (!isActiveAndEnabled) return;

        // Unsubscribe so the old instance can’t call us again
        if (currentPowerUp != null)
            currentPowerUp.OnDisabled -= HandleObjectDisabled;

        if (respawnCoroutine == null)
            respawnCoroutine = StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        float waitTime = Random.Range(minRespawnTime, maxRespawnTime);
        yield return new WaitForSeconds(waitTime);

        respawnCoroutine = null;

        if (isActiveAndEnabled)
            Spawn();
    }

    private void OnDestroy()
    {
        if (currentPowerUp != null)
            currentPowerUp.OnDisabled -= HandleObjectDisabled;
    }
}