using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAllocationManager : MonoBehaviour
{
    #region Singleton
    static EnemyAllocationManager instance;
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
    public static EnemyAllocationManager Instance { get { return instance; } }
    #endregion

    [SerializeField] int maxEnemyCount;
    [SerializeField] GameObject enemyPref;

    List<GameObject> activatedEnemies;
    Stack<GameObject> deactivatedEnemies;


    // Start is called before the first frame update
    void Start()
    {
        activatedEnemies = new List<GameObject>(250);
        deactivatedEnemies = new Stack<GameObject>();
        for(int i = 0; i < maxEnemyCount; i++)
        {
            GameObject go = Instantiate(enemyPref, new Vector3(0,-30,0), Quaternion.identity);
            go.SetActive(false);
            deactivatedEnemies.Push(go);
        }
    }


    public void DeactivateEnemy(GameObject go)
    {
        go.SetActive(false);
        go.transform.position = new Vector3(0, -30, 0);
        activatedEnemies.Remove(go);
        deactivatedEnemies.Push(go);
    }
    public GameObject GetEnemy()
    {
        return deactivatedEnemies.Pop();
    }

}
