using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    // References to external objects and components
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs; // Array of enemy prefabs for each type
    [SerializeField] private PopUpManager popupManager; // Manages popup messages

    // Spawner attributes
    [Header("Attributes")]
    // Future scaling factor for difficulty (commented out for now)
    // [SerializeField] private float difficultyScalingFactor = 0.75f;

    // Events triggered on enemy actions
    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int enemiesAlive = 0; // Keeps track of current alive enemies
    private GameVariables gameVariables; // Access game state variables
    public int numberOfEnemiesSpawned = 0; // Total spawned enemies count

    // Initialization of components and listeners
    private void Awake()
    {
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Update()
    {
        // Listen for specific button presses to trigger enemy spawning
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Spawns type 1
        {
            Debug.Log("Spawning enemy type 1");
            SpawnEnemy(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Spawns type 2
        {
            Debug.Log("Spawning enemy type 2");
            SpawnEnemy(1);
        }
        // Additional enemy types can be added with further conditions here
    }

    // Handles the event of an enemy being destroyed
    private void EnemyDestroyed()
    {
        enemiesAlive--; // Decrement alive enemy count
    }

    // Spawns an enemy of the specified type if enough resources are available
    private void SpawnEnemy(int enemyTypeIndex)
    {
        // Ensure the index is valid for the prefab array
        if (enemyTypeIndex < 0 || enemyTypeIndex >= enemyPrefabs.Length) return;

        GameObject prefabToSpawn = enemyPrefabs[enemyTypeIndex];

        // Retrieve the EnemyStats component, check for its presence
        EnemyStats enemyStats = prefabToSpawn.GetComponent<EnemyStats>();
        enemyStats.startTime = Time.time;
        if (enemyStats == null)
        {
            Debug.LogError("EnemyStats component missing on prefab: " + prefabToSpawn.name);
            return;
        }

        // Check if sufficient attack currency is available
        if (gameVariables.resourcesInfo.attackMoney >= enemyStats.cost)
        {
            gameVariables.resourcesInfo.attackMoney -= enemyStats.cost; // Deduct the cost

            // Spawn the enemy at the designated start point with default rotation
            Instantiate(prefabToSpawn, LevelManager.main.GetSelectedStartPoint().position, Quaternion.identity);
            enemiesAlive++; // Increase count of alive enemies
            numberOfEnemiesSpawned++; // Increment total spawned counter
        }
        else
        {
            // Notify player of insufficient currency to spawn the enemy
            popupManager.ShowMessage("Not enough currency to spawn enemy type " + (enemyTypeIndex + 1));
        }
    }
}
