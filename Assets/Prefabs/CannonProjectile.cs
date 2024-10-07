using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonProjectile : MonoBehaviour
{
    public float damage = 0;
    float counter = 0;

    [SerializeField] GameObject explosion;
    private void Update()
    {
        if(counter > 10)
        {
            Explode();
        }
        counter += Time.deltaTime;
    }

    void Explode()
    {
        Destroy(gameObject);
        CannonExplosion cannonExplosion =  Instantiate(explosion, transform.position, Quaternion.identity).GetComponent<CannonExplosion>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
            Explode();
    }
}
