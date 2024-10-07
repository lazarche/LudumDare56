using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    static UIManager instance;
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
    public static UIManager Instance { get { return instance; } }
    #endregion

    public CrossHair crossHair;
    public UIStatsManager statsManager;

    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] TextMeshProUGUI waveCountdown;

    [SerializeField] GameObject[] allThings;

    private void FixedUpdate()
    {
        waveText.text = "Wave: " + SpawningManager.Instance.GetCurrentWave();
        int countDown = (int)SpawningManager.Instance.GetCountdownTime();
        if (countDown > 0)
            waveCountdown.text = "Next wave starts in " + countDown;
        else
            waveCountdown.text = "";
    }

    public void HideEverything()
    {
        for (int i = 0; i < allThings.Length; i++)
            allThings[i].gameObject.SetActive(false);
    }

}
