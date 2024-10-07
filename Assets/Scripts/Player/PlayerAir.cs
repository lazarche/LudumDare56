using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAir : MonoBehaviour
{
    public float maxAir = 60;
    public float currentAir = 60;
    public float suffocatingDamage = 10; 
    PlayerHealth health;

    [SerializeField] Image imgFill;
    [SerializeField] Image imgBck;
    [SerializeField] AudioClip breathClip;

    [SerializeField] Color normalBck;
    [SerializeField] Color warningBck;

    private void Start()
    {
        health = GetComponent<PlayerHealth>();

        StartCoroutine(Breathing());
    }

    IEnumerator Breathing()
    {
        while (true)
        {
            currentAir--;
            if(currentAir < 0)
            {
                currentAir = 0;
                health.TakeDamage(suffocatingDamage);
                imgBck.transform.DOPunchScale(Vector3.one * 0.1f, 0.25f, 1);
                if(imgBck.color.Equals(normalBck))
                    imgBck.DOColor(warningBck, 1);
            }
            if (imgBck.color.Equals(warningBck))
                imgBck.color = normalBck;

            imgFill.fillAmount = currentAir / maxAir;
            yield return new WaitForSeconds(1);
        }
    }

    public void AddAir(float air)
    {
        currentAir += air;
        currentAir = Mathf.Min(currentAir, maxAir);

        SoundManager.Instance.PlaySound(breathClip, 0.6f);
    }

}
