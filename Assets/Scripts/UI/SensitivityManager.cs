using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityManager : MonoBehaviour
{
    #region Singleton
    static SensitivityManager instance;
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
    public static SensitivityManager Instance { get { return instance; } }
    #endregion

    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI text;

    public float sensitivity = 0;
    // Start is called before the first frame update
    void Start()
    {
        sensitivity = PlayerPrefs.GetFloat("sensitivity", 30);

        slider.value = sensitivity;
        text.text = Mathf.Round(sensitivity) + "";
    }

    public void UpdateSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
        slider.value = sensitivity;
        text.text = Mathf.Round(sensitivity) + "";

        PlayerPrefs.SetFloat("sensitivity", sensitivity);
        PlayerPrefs.Save();
    }

}
