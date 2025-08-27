using System.Collections;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemyGroup;

    public int rows = 4;
    public int maxRows = 6;
    public int columns = 11;
    public int maxColumns = 15;
    public float xOffset = 1.2f;
    public float yOffset = 1.2f;

    public float topPadding = 1.0f; // world units from top of screen

    private int enemyCount = 0;

    public int waveNumber = 1;

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        // Reset enemyGroup position to origin
        enemyGroup.transform.position = Vector3.zero;

        // Destroy all existing enemies (clear previous wave)
        foreach (Transform child in enemyGroup.transform)
        {
            Destroy(child.gameObject);
        }

        // Get main camera reference
        Camera cam = Camera.main;

        // Calculate screen dimensions in world units
        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        // Get enemy sprite size (including scale)
        SpriteRenderer sr = enemyPrefab.GetComponent<SpriteRenderer>();
        Vector2 spriteSize = sr.bounds.size;

        // Calculate spacing with padding
        float horizontalSpacing = spriteSize.x * xOffset;
        float verticalSpacing = spriteSize.y * yOffset;

        // Calculate total grid size
        float totalWidth = (columns - 1) * horizontalSpacing + spriteSize.x;
        float totalHeight = (rows - 1) * verticalSpacing + spriteSize.y;

        // Calculate start position (local to enemyGroup)
        float startX = -totalWidth / 2f;
        float startY = (screenHeight / 2f) - topPadding - spriteSize.y / 2f;

        // Reset enemy count
        enemyCount = 0;

        // Spawn enemies with local positions under enemyGroup
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 localPos = new Vector3(startX + col * horizontalSpacing, startY - row * verticalSpacing, 0f);

                GameObject enemyInstance = Instantiate(enemyPrefab);
                enemyInstance.transform.SetParent(enemyGroup.transform, false);
                enemyInstance.transform.localPosition = localPos;

                RegisterEnemy();
            }
        }
    }

    void RegisterEnemy()
    {
        enemyCount++;
    }

    public void UnregisterEnemy()
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            waveNumber++;
            StartNextWave();
        }
    }

    void StartNextWave()
    {
        // Move enemy group to zero in case moved
        enemyGroup.transform.position = Vector3.zero;

        // Increase rows and columns with limits
        if (rows < maxRows)
        {
            rows++;
        }

        if (columns < maxColumns)
        {
            columns++;
        }

        enemyGroup.GetComponent<EnemyController>().speed *= 1.25f;

        SpawnEnemies();
    }
}
