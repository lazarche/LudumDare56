using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int damage = 1;
    bool damaged = false;

    private void OnTriggerEnter(Collider other)
    {
        if (damaged)
            return;

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                damaged = true;
            }
        }
    }
}
