using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class AirManager : MonoBehaviour
{
    #region Singleton
    static AirManager instance;
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
    public static AirManager Instance { get { return instance; } }
    #endregion

    [SerializeField] int currentlySpawned = 0;
    [SerializeField] int maxSpawned = 33;

    [SerializeField] float timer = 0;
    [SerializeField] float spawnRate = 1;

    [SerializeField] GameObject airTankPrefab;

    [Header("Spawning")]
    [SerializeField] Transform airRegion;
    [SerializeField] float maxY = 7;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > spawnRate && currentlySpawned < maxSpawned) {
            timer = 0;
            currentlySpawned++;
            TrySpawnAir();
        }
    }

    void TrySpawnAir()
    {
        bool spawned = false;
        while(!spawned)
        {
            spawned = SpawnAir();
        }
    }

    bool SpawnAir()
    {
        Vector3 spawnPos = RandomPosition();
        if (spawnPos.y > maxY)
            return false;

        NavMeshHit hit;
        bool res = NavMesh.SamplePosition(spawnPos, out hit, 20, NavMesh.AllAreas);

        if (!res)
            return false;

        Instantiate(airTankPrefab, hit.position + new Vector3(0, 1, 0), Quaternion.identity);
        return true;
    }

    Vector3 RandomPosition()
    {
        Transform cubeTransform = airRegion;

        float minX = cubeTransform.position.x - (cubeTransform.localScale.x / 2f);
        float maxX = cubeTransform.position.x + (cubeTransform.localScale.x / 2f);

        float minZ = cubeTransform.position.z - (cubeTransform.localScale.z / 2f);
        float maxZ = cubeTransform.position.z + (cubeTransform.localScale.z / 2f);

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        float surfaceY = cubeTransform.position.y + (cubeTransform.localScale.y / 2f);

        Vector3 randomPositionOnCube = new Vector3(randomX, surfaceY, randomZ);
        return randomPositionOnCube;
    }

    public void TankCollected()
    {
        currentlySpawned--;
    }
}
