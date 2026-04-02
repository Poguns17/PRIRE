using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float cellSize = 5f;
    [SerializeField] private Transform plane;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyLimit = 5;
    [SerializeField] private float respawnDelay = 2f;

    private SpawnCell[,] grid;
    private List<GameObject> enemyPool;
    private int gridX;
    private int gridZ;

    void Start()
    {
        enemyPool = new List<GameObject>();
        CreateGrid();
        SpawnInitialEnemies();
    }

    void CreateGrid()
    {
        float planeWidth = 10f * plane.localScale.x;
        float planeLength = 10f * plane.localScale.z;

        gridX = Mathf.FloorToInt(planeWidth / cellSize);
        gridZ = Mathf.FloorToInt(planeLength / cellSize);

        grid = new SpawnCell[gridX, gridZ];

        Vector3 corner = plane.position - new Vector3(planeWidth / 2, 0, planeLength / 2);

        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                float posX = corner.x + (x * cellSize) + (cellSize / 2);
                float posZ = corner.z + (z * cellSize) + (cellSize / 2);

                Vector3 cellPosition = new Vector3(posX, 0, posZ);
                grid[x, z] = new SpawnCell(cellPosition);
            }
        }
    }

    void SpawnInitialEnemies()
    {
        for (int i = 0; i < enemyLimit; i++)
        {
            SpawnEnemy();
        }
    }

    GameObject GetEnemy()
    {
        foreach (GameObject enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
                return enemy;
        }

        if (enemyPool.Count >= enemyLimit)
            return null;

        GameObject newEnemy = Instantiate(enemyPrefab);
        enemyPool.Add(newEnemy);

        Health health = newEnemy.GetComponent<Health>();
        if (health != null)
        {
            health.OnDeath += OnEnemyDeath;
        }

        return newEnemy;
    }

    void SpawnEnemy()
    {
        Vector2Int? cellIndex = GetValidSpawnCell();
        if (cellIndex == null)
        {
            Debug.Log("No Valid Cell Found");
            return;
        }

        GameObject enemy = GetEnemy();
        if (enemy == null)
        {
            Debug.Log("No Enemy Avalaible");
            return;
        }

            int x = cellIndex.Value.x;
            int z = cellIndex.Value.y;

            enemy.transform.position = grid[x, z].WorldPosition + Vector3.up;
            enemy.SetActive(true);
            grid[x, z].IsOccupied = true;

        Debug.Log("Spawned enemy at: " + enemy.transform.position);
    }

        Vector2Int? GetValidSpawnCell()
        {
            List<Vector2Int> validCells = new List<Vector2Int>();

            for (int x = 0; x < gridX; x++)
            {
                for (int z = 0; z < gridZ; z++)
                {
                    if (!grid[x, z].IsOccupied && grid[x, z].IsSpawnable)
                    {
                        validCells.Add(new Vector2Int(x, z));
                    }
                }
            }

            if (validCells.Count == 0) return null;

            return validCells[Random.Range(0, validCells.Count)];
        }

        void OnEnemyDeath(Vector3 position, DamageType type)
        {
            StartCoroutine(HandleDeath(position));
        }

        System.Collections.IEnumerator HandleDeath(Vector3 position)
        {
            // Find and free the cell
            for (int x = 0; x < gridX; x++)
            {
                for (int z = 0; z < gridZ; z++)
                {
                    if (Vector3.Distance(grid[x, z].WorldPosition, new Vector3(position.x, 0, position.z)) < cellSize / 2)
                    {
                        grid[x, z].IsOccupied = false;
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(respawnDelay);

            SpawnEnemy();
        }
    }
