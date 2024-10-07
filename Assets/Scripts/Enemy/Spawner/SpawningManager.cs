using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawningManager : MonoBehaviour
{
    #region Singleton
    static SpawningManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    public static SpawningManager Instance { get { return instance; } }
    #endregion

    public GameObject enemyPrefab;               // The enemy prefab to spawn

    public int enemiesPerWave = 10;              // Number of enemies per wave
    public float timeBetweenSpawns = 1.0f;       // Time between individual enemy spawns
    public float waveDuration = 30f;             // Max time duration of each wave
    public float nextWaveDelay = 5f;             // Time before next wave starts if timer runs out
    public float countdownBeforeWave = 3f;       // Countdown before each wave starts

    private int currentWave = 0;                 // Current wave number
    private int enemiesAlive = 0;                // Track number of enemies alive
    private float waveTimer = 0f;                // Timer to track wave duration

    private bool waveInProgress = false;         // Whether a wave is currently in progress
    private List<Transform> spawners;            // List of combined spawners (nonvisible and visible)

    // To display the countdown
    private float countdownTimer = 0f;           // Timer for countdown before wave starts
    private bool countdownActive = false;        // Is the countdown active?

    private void Start()
    {
        StartCountdown();
    }

    private void Update()
    {
        // Countdown handling
        if (countdownActive)
        {
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0)
            {
                countdownActive = false;
                spawners = SpawnerManager.Instance.GetFourClosestSpawners();
                StartNextWave();
            }
        }

        // Handle active wave timing and progress
        if (waveInProgress)
        {
            // Decrease the wave timer
            waveTimer -= Time.deltaTime;

            // If no enemies are alive or the wave timer runs out, start the next wave
            if (enemiesAlive <= 0 || waveTimer <= 0)
            {
                StartCoroutine(NextWave());
            }
        }
    }

    // Coroutine to spawn enemies in waves over time
    private IEnumerator SpawnWave(int numEnemies)
    {
        waveInProgress = true;
        enemiesAlive = numEnemies;
        waveTimer = waveDuration;

        for (int i = 0; i < numEnemies; i++)
        {
            SpawnEnemy();

            // Wait before spawning the next enemy
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    // Function to spawn a single enemy at a random spawner
    private void SpawnEnemy()
    {
        Transform spawner = GetRandomSpawner();
        if (spawner != null)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawner.position, Quaternion.identity);
        }
    }

    private Transform GetRandomSpawner()
    {
        if (spawners.Count > 0)
        {
            int spawnerIndex = Random.Range(0, spawners.Count);
            return spawners[spawnerIndex];
        }

        return null;
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            waveTimer = 0; 
        }
    }

    // Start the next wave after the countdown
    private void StartNextWave()
    {
        currentWave++;
        StartCoroutine(SpawnWave(enemiesPerWave + currentWave * 10)); // Increase enemies each wave
    }

    // Wait for a delay before starting the next wave
    private IEnumerator NextWave()
    {
        waveInProgress = false;

        yield return new WaitForSeconds(nextWaveDelay);
        StartCountdown();
    }

    // Start the countdown before the next wave
    private void StartCountdown()
    {
        countdownTimer = countdownBeforeWave;
        countdownActive = true;
    }

    // This method can be used to display the countdown on the UI (optional)
    public float GetCountdownTime()
    {
        return Mathf.Max(0, countdownTimer); // Return the remaining countdown time
    }

    public float GetCurrentWave()
    {
        return currentWave;
    }
}
