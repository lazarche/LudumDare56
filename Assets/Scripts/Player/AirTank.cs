using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTank : MonoBehaviour
{
    [SerializeReference] Transform laser;
    Transform player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating("ChangeSize", 0, 1);
    }

    void ChangeSize()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        
        float newSize = 0.03f + (Mathf.Min(100,Mathf.Max(0,distance-15))/100f);
        
        Vector3 newScale = new Vector3(newSize,500,newSize);
        laser.localScale = newScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerAir playerAir = other.GetComponent<PlayerAir>();
            if (playerAir != null)
            {
                playerAir.AddAir(60);
                AirManager.Instance.TankCollected();
                Destroy(gameObject);
            }
        }
    }
}
