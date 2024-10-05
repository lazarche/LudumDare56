using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    [SerializeField] GameObject enemyPref;
    [SerializeField] int numberOfEnemies = 100;


    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPref, EnemyHelper.SpawnOnNavmesh(new Vector3(0, 1.5f, 0)), Quaternion.identity);

    }

    IEnumerator SpawnEnemies()
    {
        int count = 5;
        while(true)
        {
            for(int i = 0; i < count; i++)
                SpawnEnemy();

            count++;
            yield return new WaitForSeconds(4);
        }
    }

}
