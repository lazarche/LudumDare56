using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    PlayerMotor playerMotor;

    [SerializeField] AudioSource footSteps;
    [SerializeField] AudioClip[] jump;
    [SerializeField] AudioClip land;

    private void Start()
    {
        playerMotor = GetComponent<PlayerMotor>();
        playerMotor.jumped += Jumped;
    }

    private void Jumped()
    {
        Invoke("InAirDelayed", 0);
        SoundManager.Instance.PlaySound(jump[Random.Range(0, jump.Length)], 0.4f);
    }

    void InAirDelayed()
    {
        inAir = true;
    }
    

    bool inAir = false;
    private void Update()
    {
        if (playerMotor.controller.isGrounded && playerMotor.moveDirection.magnitude > 0)
            footSteps.enabled = true;
        else
            footSteps.enabled = false;

        footSteps.pitch = playerMotor.sprinting ? 1.3f : 1;


        if(inAir && playerMotor.controller.isGrounded)
        {
            inAir = false;
            SoundManager.Instance.PlaySound(land, 0.6f);
        }

    }

}
