using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AssaultRifle : Gun
{
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            TryShoot();
        } else
        {
            RecoverAccuracy();
        }

        if(Input.GetKey(KeyCode.R)) {
            StartReload();
        }
    }

    public override void Shoot()
    {
        base.Shoot();

        RaycastHit hit;
        Vector3 randomDirection = cam.transform.forward +
                                  new Vector3(
                                      Random.Range(-GetAccuracy() / 2, GetAccuracy() / 2),
                                      Random.Range(-GetAccuracy() / 2, GetAccuracy() / 2),
                                      Random.Range(-GetAccuracy() / 2, GetAccuracy() / 2));

        UIManager.Instance.crossHair.UpdateCrossHair(currentAccuracy / maxInaccuracy);

        if (Physics.Raycast(cam.transform.position, randomDirection, out hit, range))
        {
            //sooting
            Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.TakeDamage(StatsManager.Instance.Damage);
            } 
            else
            {
                GameObject mark = GunMarkManager.Instance.GetMark();
                mark.transform.position = hit.point + hit.normal * 0.005f; ;
                mark.transform.rotation = Quaternion.LookRotation(hit.normal);
            }
        }

        Debug.Log("shoot");
        currentMag--;
    }
}
