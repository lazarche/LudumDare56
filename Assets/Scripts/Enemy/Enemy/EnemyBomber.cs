using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomber : Enemy
{
    [SerializeField] AudioSource chargeSound;
    [SerializeField] GameObject explosion;
    protected override void Attack()
    {
        chargeSound.Play();
        stunned = 500;
        Invoke("Explode", 1);
    }

    void Explode()
    {
        Explosion exp =  Instantiate(explosion, transform.position, Quaternion.identity).GetComponent<Explosion>();
        exp.damage = attackDamage;
        Destroy(gameObject);
        Debug.Log("EXPLOSION");
    }
}
