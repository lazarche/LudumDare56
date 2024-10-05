using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFootActions;

    private PlayerMotor playerMotor;
    private PlayerLook playerLook;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFootActions = playerInput.OnFoot;

        playerMotor = GetComponent<PlayerMotor>();
        playerLook = GetComponent<PlayerLook>();

        onFootActions.Jump.performed += ctx => playerMotor.Jump();
    }

    void Update()
    {
        playerMotor.ProcessMove(onFootActions.Movement.ReadValue<Vector2>());    
    }

    private void LateUpdate()
    {
        playerLook.ProcessLook(onFootActions.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFootActions.Enable();   
    }
    private void OnDisable()
    {
        onFootActions.Disable();
    }
}
