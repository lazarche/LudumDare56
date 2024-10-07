using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cannon : MonoBehaviour
{
    public bool canFire = true;
    public float currentColdown = 0;

    public float attackDamage = 60;
    public float area = 5;

    public Transform launchPoint;
    public GameObject projectilePrefab;

    public Image fillBar;

    public Camera cam;

    public CharacterController characterController;

    private void Start()
    {
        cam = Camera.main;
        currentColdown = StatsManager.Instance.CannonReload;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            TryShoot();
        }

        if(!canFire)
        {
            currentColdown += Time.deltaTime;
            if(currentColdown > StatsManager.Instance.CannonReload)
            {
                canFire = true;
            }
        }

        fillBar.fillAmount = Mathf.Clamp01(currentColdown / StatsManager.Instance.CannonReload);
    }

    private void TryShoot()
    {
        if (canFire)
            Shoot();
    }

    private void Shoot()
    {
        canFire = false;
        currentColdown = 0;

        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
        projectile.GetComponent<CannonProjectile>().damage = attackDamage;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.velocity = characterController.velocity + cam.transform.forward * 20;
    }
}
