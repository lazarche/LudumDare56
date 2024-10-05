using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHelper
{
    public static Vector3 SpawnOnNavmesh(Vector3 pos)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 100, NavMesh.AllAreas))
            return hit.position;
        else
        {
            Debug.LogError("kurcina");
            return Vector3.zero;
        }
    }
}
