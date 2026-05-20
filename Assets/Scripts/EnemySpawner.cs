using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Object settings")]
    public GameObject enemyPrefab;
    public string enemyTag = "Enemy";
    public int maxEnemies = 15;

    [Header("Spawn settings")]
    public float spawnInterval = 3f;
    public int minToSpawn = 1;
    public int maxToSpawn = 3;

    [Header("Zone settings")]
    public LayerMask obstacleLayer;
    public float checkRadius = 0.5f;

    private Collider2D zoneCollider;
    private bool isPlayerInZone = false;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        zoneCollider = GetComponent<Collider2D>();

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (isPlayerInZone)
            {
                int currentEnemyCount = GameObject.FindGameObjectsWithTag(enemyTag).Length;

                if (currentEnemyCount < maxEnemies)
                {
                    int amountToSpawn = Random.Range(minToSpawn, maxToSpawn + 1);

                    for (int i = 0; i < amountToSpawn; i++)
                    {
                        if (currentEnemyCount >= maxEnemies) break;

                        Vector3 spawnPos = GetValidSpawnPosition();

                        // If there is a valid point
                        if (spawnPos != Vector3.zero)
                        {
                            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                            currentEnemyCount++;
                        }
                    }
                }
            }
        }
    }

    Vector3 GetValidSpawnPosition()
    {
        Bounds bounds = zoneCollider.bounds;
        int attempts = 0;

        while (attempts < 20)
        {
            // Chose a random spawnpoint inside EnemyManager collider bounds
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            Vector3 candidatePoint = new(x, y, 0);

            // Spawnpoint outside of camera view
            Vector3 screenPoint = mainCamera.WorldToViewportPoint(candidatePoint);
            bool isVisible = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            if (isVisible)
            {
                attempts++;
                continue;
            }

            // No obstacles on spawnpoint
            Collider2D obstacle = Physics2D.OverlapCircle(candidatePoint, checkRadius, obstacleLayer);
            if (obstacle != null)
            {
                attempts++;
                continue;
            }

            return candidatePoint;
        }

        return Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerInZone = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerInZone = false;
    }
}