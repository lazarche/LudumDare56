using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    static GameManager instance;
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
    public static GameManager Instance { get { return instance; } }
    #endregion

    [SerializeField] SpawningManager spawningManager;
    [SerializeField] GameObject player;

    public void GameOver()
    {
        UIManager.Instance.HideEverything();
        spawningManager.enabled = false;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
            enemies[i].GetComponent<Enemy>().enabled=false;
    }
}
