using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        SpawnerManager.Instance.RemoveSpawner(this);
    }

    private void OnBecameVisible()
    {
        SpawnerManager.Instance.AddSpawner(this);
    }
}
