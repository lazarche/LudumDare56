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
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(1);
        Explosion exp =  Instantiate(explosion, transform.position, Quaternion.identity).GetComponent<Explosion>();
        exp.damage = attackDamage;
        Destroy(gameObject);
        Debug.Log("EXPLOSION");
        yield return null;
    }
}
