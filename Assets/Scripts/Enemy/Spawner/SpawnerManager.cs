using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    #region Singleton
    static SpawnerManager instance;
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
    public static SpawnerManager Instance { get { return instance; } }
    #endregion

    public List<Spawner> spawners = new List<Spawner>();
    public List<Spawner> visibleSpawners = new List<Spawner>();
    public List<Spawner> nonvisibleSpawners = new List<Spawner>();
    public List<Spawner> validSpawners = new List<Spawner>();

    public Transform player;

    public void AddSpawner(Spawner spawner)
    {
        visibleSpawners.Add(spawner);
        nonvisibleSpawners.Remove(spawner);
    }

    public void RemoveSpawner(Spawner spawner)
    {
        visibleSpawners.Remove(spawner);
        nonvisibleSpawners.Add(spawner);
    }

    public List<Transform> GetFourClosestSpawners()
    {
        // Sort both lists by proximity to the player
        var sortedNonVisible = nonvisibleSpawners.OrderBy(x => Vector3.Distance(x.transform.position, player.position)).ToList();
        var sortedVisible = visibleSpawners.OrderBy(x => Vector3.Distance(x.transform.position, player.position)).ToList();

        List<Spawner> selectedSpawners = new List<Spawner>();

        // Try to get two spawners from non-visible list
        int nonVisibleCount = Mathf.Min(2, sortedNonVisible.Count); // Get up to 2 if available
        selectedSpawners.AddRange(sortedNonVisible.Take(nonVisibleCount));

        // Try to get two spawners from visible list
        int visibleCount = Mathf.Min(2, sortedVisible.Count); // Get up to 2 if available
        selectedSpawners.AddRange(sortedVisible.Take(visibleCount));

        // If we still need more spawners to reach a total of 4
        int remainingSpawnersNeeded = 4 - selectedSpawners.Count;

        if (remainingSpawnersNeeded > 0)
        {
            // Fill in the rest from whichever list still has remaining spawners
            if (nonVisibleCount < 2)
            {
                selectedSpawners.AddRange(sortedNonVisible.Skip(nonVisibleCount).Take(remainingSpawnersNeeded));
            }

            if (selectedSpawners.Count < 4 && visibleCount < 2)
            {
                int stillNeeded = 4 - selectedSpawners.Count;
                selectedSpawners.AddRange(sortedVisible.Skip(visibleCount).Take(stillNeeded));
            }
        }


        List<Transform> toReturn = new List<Transform>();
        for(int i = 0; i < selectedSpawners.Count; i++)
        {
            toReturn.Add(selectedSpawners[i].transform);
        }

        // Ensure we return exactly 4 spawners
        return toReturn;
    }
}
