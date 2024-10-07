using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public CharacterController controller;
    public Vector3 playerVelocity;
    public float speed = 5f;
    public float gravity = -100f;
    public float jumpHeight = 3f;

    bool lerpCrouch = false;
    float crouchTimer = 0f;
    public bool crouching = false;
    public bool sprinting = false;
    public Action jumped;
    public Vector3 moveDirection = Vector3.zero;
    public bool Walking { get { return moveDirection.magnitude > 0; } }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        if(lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if(crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if(p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0;
            }
        }
    }
    public void ProcessMove(Vector2 input)
    {
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        float currentSpeed = speed;
        if (crouching)
            currentSpeed *= 0.7f;
        else if(sprinting)
            currentSpeed *= 1.4f;

        controller.Move(transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime);
        if(!controller.isGrounded)
            playerVelocity.y += gravity * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);

    }
    public void Jump()
    {
        if(controller.isGrounded)
        {
            jumped?.Invoke();
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
    public void Crouch(bool crouching)
    {
        this.crouching = crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }
    public void ToggleSprinting(bool sprinting)
    {
        this.sprinting = sprinting;
        Debug.Log("Sprinting: " + this.sprinting);
    }
}
