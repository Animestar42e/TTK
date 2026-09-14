using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject enemyPrefab;

    [Header("Spawn Settings")]
    public float spawnTime = 3f;
    public int maxEnemies = 10;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        // Check enemy prefab
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner: Enemy Prefab is not assigned!");
            return;
        }

        // Check spawn points
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("EnemySpawner: No Spawn Points assigned!");
            return;
        }

        // Count existing enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length >= maxEnemies)
        {
            return;
        }

        // Pick a random spawn point
        int randomPoint = Random.Range(0, spawnPoints.Length);

        Transform spawnPoint = spawnPoints[randomPoint];

        if (spawnPoint == null)
        {
            Debug.LogWarning("EnemySpawner: One of your Spawn Points is empty!");
            return;
        }

        // Spawn enemy
        GameObject newEnemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        // Make sure spawned enemy has the Enemy tag
        newEnemy.tag = "Enemy";
    }
}
