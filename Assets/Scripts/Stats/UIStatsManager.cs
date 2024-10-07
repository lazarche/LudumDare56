using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStatsManager : MonoBehaviour
{
    [Header("Stats window")]
    [SerializeField] GameObject statPanel;
    [SerializeField] GameObject statText;

    [Header("Stats level up")]
    [SerializeField] GameObject statLevelUp;
    [SerializeField] Transform statLevelUpHolder;
    [SerializeField] GameObject buttonPref;

    [Header("Stats")]
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI hpRegenText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI fireRateText;
    [SerializeField] TextMeshProUGUI magazineText;
    [SerializeField] TextMeshProUGUI reloadText;

    public bool canOpenStat = true;
    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab) && canOpenStat)
        {
            UpdateStatPanel();
            statPanel.SetActive(true);
            statText.SetActive(false);
            statPanel.transform.SetAsLastSibling();
        } else
        {
            statPanel.SetActive(false);
            statText.SetActive(true);
        }
    }

    void UpdateStatPanel()
    {
        hpText.text = $"Health: {StatsManager.Instance.Health} (lvl {StatsManager.Instance.healthLevel}) ";
        hpRegenText.text = $"Health Regen: {StatsManager.Instance.HpRegen} (lvl {StatsManager.Instance.hpRegenLevel}) ";
        speedText.text = $"Speed: {StatsManager.Instance.Speed} (lvl {StatsManager.Instance.speedLevel}) ";
        damageText.text = $"Damage: {StatsManager.Instance.Damage} (lvl {StatsManager.Instance.damageLevel}) ";
        fireRateText.text = $"Fire Rate: {StatsManager.Instance.FireRate} (lvl {StatsManager.Instance.fireRateLevel}) ";
        magazineText.text = $"Magazine Size: {StatsManager.Instance.MagazineSize} (lvl {StatsManager.Instance.magazineSizeLevel}) ";
        reloadText.text = $"Reload Speed: {StatsManager.Instance.ReloadSpeed} (lvl {StatsManager.Instance.reloadSpeedLevel}) ";

        LayoutRebuilder.ForceRebuildLayoutImmediate(statPanel.GetComponent<RectTransform>());
    }

    public void ShowLevelUpScreen()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;

        statLevelUp.SetActive(true);

        for(int i = 1; i < statLevelUpHolder.childCount; i++)
            Destroy(statLevelUpHolder.GetChild(i).gameObject);

        string[] statSelection = StatsManager.Instance.GetRandomStats();

        foreach(string stat in statSelection)
        {
            GameObject button = Instantiate(buttonPref, statLevelUpHolder);
            StatLevelUpButton holder = button.GetComponent<StatLevelUpButton>();
            holder.title.text = StatsManager.Instance.statName[stat];
            holder.description.text = StatsManager.Instance.statDescription[stat];
            holder.level.text = "Level " + StatsManager.Instance.GetLevel(stat);

            button.GetComponent<Button>().onClick.AddListener( delegate { SelectStat(stat); });
        }
    }

    public void SelectStat(string stat)
    {
        Time.timeScale = 1;
        SoundManager.Instance.PlaySound("buttonClick");
        Cursor.lockState = CursorLockMode.Locked;
        StatsManager.Instance.IncreaseStat(stat);
        statLevelUp.SetActive(false);
    }
}
