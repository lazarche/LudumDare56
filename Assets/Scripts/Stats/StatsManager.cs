using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    #region Singleton
    static StatsManager instance;
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
    public static StatsManager Instance { get { return instance; } }
    #endregion

    public int healthLevel = 0;
    public float Health {  get { return 100f + healthLevel * 10f; } }

    public int hpRegenLevel = 0;
    public float HpRegen { get {  return hpRegenLevel * 0.6f; } }

    public int speedLevel = 0;
    public float Speed { get { return 3 + speedLevel * 0.12f; } }

    public int damageLevel = 0;
    public float Damage { get { return 30 + damageLevel * 5; } }

    public int fireRateLevel = 0;
    public float FireRate { get { return 1f/ (float)(3 + (fireRateLevel * 0.5f)); } }

    public int magazineSizeLevel = 0;
    public int MagazineSize { get { return 15 + magazineSizeLevel * 5; } }

    public int reloadSpeedLevel = 0;
    public float ReloadSpeed { get { return Mathf.Max(0, 4 - (reloadSpeedLevel * 0.15f)); } }

    List<string> stats = new List<string>
    {
        "health",
        "healthRegen",
        "speed",
        "damage",
        "firerate",
        "magazine",
        "reload"
    };

    public Dictionary<string, string> statDescription = new Dictionary<string, string>
    {
        {"health", "Increases max player Health by 7 for each level"},
        {"healthRegen", "Players Health recovers faster, 0.6 health/sec per level"},
        {"speed", "Increases movement speed of player, 0.12 m/s per level"},
        {"damage", "Gun deals more damage, 5 for each level"},
        {"firerate", "Gun shoots faster, 0.5 bullets/second per level"},
        {"magazine", "Guns magazine hold more bullets, 5 more bullets per level"},
        {"reload", "Player reloads gun faster, -0.15 seconds to reload per level"}

    };

    public Dictionary<string, string> statName = new Dictionary<string, string>
    {
        {"health", "Health"},
        {"healthRegen", "Health Regen"},
        {"speed", "Movespeed"},
        {"damage", "Damage"},
        {"firerate", "Fire Rate"},
        {"magazine", "Magazine"},
        {"reload", "Reload"}

    };


    public void IncreaseStat(string statName)
    {
        switch(statName)
        {
            case "health":
                healthLevel++;
                break;
            case "healthRegen":
                hpRegenLevel++;
                break;
            case "speed":
                speedLevel++;
                break;
            case "damage":
                damageLevel++;
                break;
            case "firerate":
                fireRateLevel++;
                break;
            case "magazine":
                magazineSizeLevel++;
                break;
            case "reload":
                reloadSpeedLevel++;
                break;
        }
    }

    public int GetLevel(string statName)
    {
        switch (statName)
        {
            case "health":
                return healthLevel;
            case "healthRegen":
                return hpRegenLevel;
            case "speed":
                return speedLevel;
            case "damage":
                return damageLevel;
            case "firerate":
                return fireRateLevel;
            case "magazine":
                return magazineSizeLevel;
            case "reload":
                return reloadSpeedLevel;
        }
        return -1;
    }

    public string[] GetRandomStats()
    {
        List<string> tempStats = new List<string>(stats);

        string[] toReturn = new string[3];

        for (int i = 0; i < 3; i++) {
            int index = Random.Range(0, tempStats.Count);
            toReturn[i] = tempStats[index];
            tempStats.RemoveAt(index);
        }

        return toReturn;
    }
}
