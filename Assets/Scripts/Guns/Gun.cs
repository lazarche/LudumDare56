using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] AudioSource gunSound;
    [SerializeField] Animator animator;
    [SerializeField] PlayerMotor playerMotor;

    public Camera cam;


    [Header("Stats")]
    public int maxAmmo = 300;
    public int magSize = 30;
    public int currentMag = 30;
    public float fireRate = 0.15f;
    public float range = 50;
    public float damage = 60f;

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

    private void Awake()
    {
        cam = Camera.main;
    }
    public virtual bool CanShoot()
    {
        if (currentMag > 0 && !onColdown && !reloading)
            return true;
        return false;
    }
    public virtual void TryShoot() {
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
        Invoke("ShootColdown", fireRate);
    }
    void ShootColdown()
    {
        onColdown = false;
        if(currentMag == 0)
            StartReload();
    }
    public virtual void StartReload()
    {
        if (reloading || maxAmmo == 0)
            return;

        reloading = true;
        animator.Play("Reload");
    }
    public virtual void Reload() {


        int needBullets = magSize - currentMag;

        if (needBullets > maxAmmo)
        {
            currentMag += maxAmmo;
            maxAmmo = 0;
        } else
        {
            currentMag += needBullets;
            maxAmmo -= needBullets;
        }

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
