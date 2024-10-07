using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsManager : MonoBehaviour
{
    #region Singleton
    static EnemyStatsManager instance;
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
    public static EnemyStatsManager Instance { get { return instance; } }
    #endregion

    public int healthLevel = 0;
    public int speedLevel = 0;
    public int damageLevel = 0;
    public int expLevel = 0;
}
