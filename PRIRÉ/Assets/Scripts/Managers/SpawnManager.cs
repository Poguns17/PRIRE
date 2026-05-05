using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float cellSize = 5f;
    [SerializeField] private Transform plane;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyLimit = 5;
    [SerializeField] private float respawnDelay = 2f;
    [SerializeField] private LayerMask groundLayer;

    private SpawnCell[,] grid;
    private List<GameObject> enemyPool;
    private int gridX;
    private int gridZ;

    void Start()
    {
        enemyPool = new List<GameObject>();
        CreateGrid();
        SpawnInitialEnemies();
        //Debug.Log($"Plane name: {plane.name}");
        //Debug.Log($"Plane position: {plane.position}");
        //Debug.Log($"Plane scale: {plane.localScale}");
    }

    void CreateGrid()
    {

        Renderer groundRenderer = plane.GetComponent<Renderer>();

        float planeWidth;
        float planeLength;
        Vector3 center;

        if (groundRenderer != null)
        {

            Bounds bounds = groundRenderer.bounds;
            planeWidth = bounds.size.x;
            planeLength = bounds.size.z;
            center = bounds.center;

            Debug.Log($"Using Renderer bounds: {bounds.size}");
        }
        else
        {

            planeWidth = 10f * plane.lossyScale.x;
            planeLength = 10f * plane.lossyScale.z;
            center = plane.position;

            Debug.Log($"Using scale calculation");
        }

        gridX = Mathf.FloorToInt(planeWidth / cellSize);
        gridZ = Mathf.FloorToInt(planeLength / cellSize);

        grid = new SpawnCell[gridX, gridZ];


        Vector3 corner = center - new Vector3(planeWidth / 2, 0, planeLength / 2);

        Debug.Log($"Center: {center}");
        Debug.Log($"Plane width: {planeWidth}, length: {planeLength}");
        Debug.Log($"Grid size: {gridX} x {gridZ}");
        Debug.Log($"Corner: {corner}");

        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                float posX = corner.x + (x * cellSize) + (cellSize / 2);
                float posZ = corner.z + (z * cellSize) + (cellSize / 2);

                Vector3 cellPosition = new Vector3(posX, center.y, posZ);

                SpawnCell cell = new SpawnCell(cellPosition);
                cell.IsSpawnable = IsCellValid(cellPosition);

                grid[x, z] = cell;


            }
        }

        Debug.Log($"First cell: {grid[0, 0].WorldPosition}");
        Debug.Log($"Last cell: {grid[gridX - 1, gridZ - 1].WorldPosition}");
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
                Vector2 cellPos = new Vector2(
                grid[x, z].WorldPosition.x,
                grid[x, z].WorldPosition.z
                );

                Vector2 deathPos = new Vector2(
                position.x,
                position.z
                );

                if (Vector2.Distance(cellPos, deathPos) < cellSize / 2)
                {
                    grid[x, z].IsOccupied = false;
                    break;
                }
            }
        }

        yield return new WaitForSeconds(respawnDelay);

        SpawnEnemy();
    }

    bool IsCellValid(Vector3 position)
    {
        float rayHeight = 10f;

        Vector3 rayStart = position + Vector3.up * rayHeight;

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, rayHeight * 2, groundLayer))
        {
            Debug.Log($"Valid ground hit: {hit.transform.name}");
            return true;
        }

        Debug.Log($"Invalid cell (no ground hit) at {position}");
        return false;
    }

    void OnDrawGizmos()
    {
        if (grid == null) return;

        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                if (grid[x, z].IsOccupied)
                    Gizmos.color = Color.red;
                else if (grid[x, z].IsSpawnable)
                    Gizmos.color = Color.green;
                else
                    Gizmos.color = Color.gray;

                Gizmos.DrawWireCube(
                    grid[x, z].WorldPosition + Vector3.up * 10f,
                    new Vector3(cellSize * 0.9f, 0.2f, cellSize * 0.9f)
                );
            }
        }
    }
}
