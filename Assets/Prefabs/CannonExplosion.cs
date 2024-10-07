using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonExplosion : MonoBehaviour
{
    [SerializeField] GameObject particles;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("damage aaa");
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(StatsManager.Instance.CannonDamage);
            }
            
        }
    }

    private void Start()
    {
        Invoke("DestroySelf", 0.05f);
        Instantiate(particles, transform.position, Quaternion.identity);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
