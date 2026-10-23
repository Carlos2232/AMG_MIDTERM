using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject chaserPrefab;
    public GameObject tankPrefab;
    public GameObject shooterPrefab;
    public GameObject crawlerPrefab;
    public GameObject miniBossPrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 0.2f;
    private float spawnTimer = 0f;

    [Header("Mini-Boss Spawn Settings")]
    public float miniBossInterval = 60f;
    private float miniBossTimer = 0f;

    [Header("Difficulty Scaling (Over Time)")]
    public float difficultyScalingInterval = 60f;
    private float difficultyTimer = 0f;
    private int difficultyTier = 0;

    void Update()
    {
        miniBossTimer += Time.deltaTime;
        if (miniBossTimer >= miniBossInterval)
        {
            SpawnMiniBoss();
            miniBossTimer = 0f;
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnRandomRegularEnemy();
            spawnTimer = 0f;
        }

        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= difficultyScalingInterval)
        {
            difficultyTier++;
            difficultyTimer = 0f;
            Debug.Log("Survival Difficulty Tier Increased: " + difficultyTier);
        }
    }

    private Vector2 GetOffScreenSpawnPosition(Transform playerTransform, float spawnDistanceOffset = 2f)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            return (Vector2)playerTransform.position + Random.insideUnitCircle.normalized * 12f;
        }

        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;

        int side = Random.Range(0, 4);
        Vector2 spawnPos = Vector2.zero;
        Vector2 playerPos = playerTransform.position;

        switch (side)
        {
            case 0: 
                spawnPos = new Vector2(Random.Range(-horzExtent, horzExtent), vertExtent + spawnDistanceOffset);
                break;
            case 1: 
                spawnPos = new Vector2(Random.Range(-horzExtent, horzExtent), -vertExtent - spawnDistanceOffset);
                break;
            case 2: 
                spawnPos = new Vector2(-horzExtent - spawnDistanceOffset, Random.Range(-vertExtent, vertExtent));
                break;
            case 3: 
                spawnPos = new Vector2(horzExtent + spawnDistanceOffset, Random.Range(-vertExtent, vertExtent));
                break;
        }

        return playerPos + spawnPos;
    }

    void SpawnRandomRegularEnemy()
    {
        GameObject[] regularPool = { chaserPrefab, tankPrefab, shooterPrefab, crawlerPrefab };
        GameObject chosenPrefab = regularPool[Random.Range(0, regularPool.Length)];

        if (chosenPrefab != null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj == null) return;

            Vector2 spawnPos = GetOffScreenSpawnPosition(playerObj.transform, 3f);

            GameObject enemy = null;
            if (ObjectPooler.Instance != null)
            {
                enemy = ObjectPooler.Instance.SpawnFromPool(chosenPrefab.name, spawnPos, Quaternion.identity);
            }

            if (enemy == null)
            {
                enemy = Instantiate(chosenPrefab, spawnPos, Quaternion.identity);
            }

            EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
            if (enemyBase != null)
            {
                float scaledHP = enemyBase.maxHP * Mathf.Pow(1.2f, difficultyTier);
                float scaledSpeed = enemyBase.moveSpeed * Mathf.Pow(1.05f, difficultyTier);
                enemyBase.InitializeStats(scaledHP, scaledSpeed, difficultyTier + 1);
            }
        }
    }

    void SpawnMiniBoss()
    {
        if (miniBossPrefab != null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj == null) return;

            Vector2 spawnPos = GetOffScreenSpawnPosition(playerObj.transform, 4f);

            GameObject boss = null;
            if (ObjectPooler.Instance != null)
            {
                boss = ObjectPooler.Instance.SpawnFromPool(miniBossPrefab.name, spawnPos, Quaternion.identity);
            }

            if (boss == null)
            {
                boss = Instantiate(miniBossPrefab, spawnPos, Quaternion.identity);
            }

            Debug.Log("Mini-Boss has spawned!");
        }
    }
}