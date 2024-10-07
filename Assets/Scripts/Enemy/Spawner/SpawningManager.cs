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

    public GameObject enemyMeleePrefab;
    public GameObject enemyRangePrefab;
    public GameObject enemyBomberPrefab;

    public int enemiesPerWave = 10;
    public float timeBetweenSpawns = 1.0f;
    public float waveDuration = 30f;
    public float nextWaveDelay = 5f;
    public float countdownBeforeWave = 3f;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private float waveTimer = 0f;

    private bool waveInProgress = false;
    private List<Transform> spawners;

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
            int chance = Random.Range(0, 100);

            if(GetCurrentWave() > 2 && chance > 90)
                Instantiate(enemyBomberPrefab, spawner.position, Quaternion.identity);
            else if (GetCurrentWave() > 1 && chance > 60)
                Instantiate(enemyRangePrefab, spawner.position, Quaternion.identity);
            else
                Instantiate(enemyMeleePrefab, spawner.position, Quaternion.identity);
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
        if (currentWave % 3 == 0)
            EnemyStatsManager.Instance.IncreaseDiff(1);
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
