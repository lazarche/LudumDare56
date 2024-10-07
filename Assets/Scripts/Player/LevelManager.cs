using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    static LevelManager instance;
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
    public static LevelManager Instance { get { return instance; } }
    #endregion

    public int level = 1;

    public int currentExp = 0;
    public int currentMaxExp = 1000;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI levelTitle;
    [SerializeField] Image levelBarFill;

    public void AddExperience(int experience)
    {
        currentExp += experience;

        if(currentExp > currentMaxExp)
        {
            currentExp = currentExp - currentMaxExp;
            currentMaxExp = (int)(currentMaxExp * 1.001f) + ((level+1) * 100);
            level++;

            UIManager.Instance.statsManager.ShowLevelUpScreen();
        }

        levelTitle.text = "Level " + level;
        levelBarFill.fillAmount = (float) currentExp / (float) currentMaxExp;
    }
}
