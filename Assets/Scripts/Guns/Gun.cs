using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] AudioSource gunSound;
    [SerializeField] Animator animator;
    [SerializeField] PlayerMotor playerMotor;


    public Camera cam;

    [Header("Stats")]
    public int currentMag = 0;
    public float range = 50;

    [Header("Accuracy")]
    public float currentAccuracy;
    public float baseAccuracy = 0f;
    public float maxInaccuracy = 1f;
    public float accuracyDecayRate = 0.01f;
    public float shootingRecoveryRate = 0.5f;
    public float movementInaccuracy = 0.15f;
    public float sprintInaccuracy = 0.30f;
    public float crouchInaccuracy = 0.05f;
    private float shootingTime;

    [Header("States")]
    public bool onColdown = false;
    public bool reloading = false;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI currentMagText;
    [SerializeField] TextMeshProUGUI maxMagText;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        currentMag = StatsManager.Instance.MagazineSize;
    }

    private void FixedUpdate()
    {
        currentMagText.text = currentMag + "";
        maxMagText.text = "/"+StatsManager.Instance.MagazineSize;

        LayoutRebuilder.ForceRebuildLayoutImmediate(currentMagText.transform.parent.gameObject.GetComponent<RectTransform>());
    }

    public virtual bool CanShoot()
    {
        if (currentMag > 0 && !onColdown && !reloading)
            return true;
        return false;
    }
    public virtual void TryShoot() {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        if(CanShoot())
            Shoot();
    }
    public virtual void Shoot() {
        
        shootingTime += Time.deltaTime;
        gunSound.Play();
        muzzleFlash.Play();
        animator.Play("Shoot");
        onColdown = true;
        UpdateAccuracy();
        Invoke("ShootColdown", StatsManager.Instance.FireRate);
    }
    void ShootColdown()
    {
        onColdown = false;
        if(currentMag == 0)
            StartReload();
    }
    public virtual void StartReload()
    {
        if (reloading || currentMag == StatsManager.Instance.MagazineSize)
            return;

        reloading = true;
        animator.Play("Reload");
    }
    public virtual void Reload() {
        currentMag = StatsManager.Instance.MagazineSize;

        reloading = false;
    }
    public void UpdateAccuracy()
    {
        currentAccuracy += accuracyDecayRate * shootingTime;

        if(playerMotor.sprinting)
            currentAccuracy += sprintInaccuracy;
        if (playerMotor.crouching)
            currentAccuracy *= crouchInaccuracy;
        if(playerMotor.Walking)
            currentAccuracy += movementInaccuracy;

            currentAccuracy = Mathf.Clamp(currentAccuracy, baseAccuracy, maxInaccuracy);
    }
    public void RecoverAccuracy()
    {
        currentAccuracy -= shootingRecoveryRate * Time.deltaTime;
        currentAccuracy = Mathf.Clamp(currentAccuracy, baseAccuracy, maxInaccuracy);
        UIManager.Instance.crossHair.UpdateCrossHair(currentAccuracy / maxInaccuracy);
    }

    public float GetAccuracy()
    {
        return currentAccuracy * 0.1f;
    }
}
