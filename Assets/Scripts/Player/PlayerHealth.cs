using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    float currentHp = 100;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI hpMaxText;
    [SerializeField] Image hpBar;

    [SerializeField] AudioClip[] hurtSound;
    [SerializeField] AudioClip deathSound;

    bool dead = false;

    private void Start()
    {
        InvokeRepeating("Regen", 1, 1);
    }

    void Regen()
    {
        if (dead)
            return;

        currentHp += StatsManager.Instance.HpRegen;
        if(currentHp > StatsManager.Instance.Health)
            currentHp = StatsManager.Instance.Health;
        UpdateText();
    }

    void UpdateText()
    {
        hpText.text = Mathf.FloorToInt(currentHp).ToString();
        hpMaxText.text = "/"+ Mathf.FloorToInt(StatsManager.Instance.Health);
        hpBar.fillAmount = currentHp / StatsManager.Instance.Health;

        LayoutRebuilder.ForceRebuildLayoutImmediate(hpText.transform.parent.GetComponent<RectTransform>());
    }

    public void TakeDamage(float damage)
    {
        if (dead)
            return;

        currentHp -= damage;
        if(currentHp < 0)
        {
            currentHp = 0;
            SoundManager.Instance.PlaySound(deathSound);
            Die();
        } else
            SoundManager.Instance.PlaySound(hurtSound[Random.Range(0, hurtSound.Length)], 0.6f);

        UpdateText();
    }

    private void Die()
    {
        dead = true;
        GameManager.Instance.GameOver();
        Debug.Log("Mrtav sam");
    }
}
