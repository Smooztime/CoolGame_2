using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public event Action OnInteract;
    private PlayerController _playerController;
    private PlayerInput _playerInput;


    private void OnEnable()
    {
        _playerController = GetComponent<PlayerController>();

        if (_playerController == null) return;

        _playerInput = new PlayerInput();
        _playerInput.PlayerMovement.Movement.performed += value => _playerController.MovementInput(value.ReadValue<Vector2>()); //Add Character movement
        _playerInput.PlayerMovement.Aim.performed += value => _playerController.MousePosition(value.ReadValue<Vector2>()); //Rotate Character by cursor
        _playerInput.PlayerMovement.Dash.performed += value => _playerController.Dashing(); //Press shift to dash
        _playerInput.PlayerMovement.Fire.performed += value => _playerController.WeaponShoot(); //Gun fire
        _playerInput.PlayerMovement.HoldFire.performed += value => _playerController.WeaponHoldShootInput(value.ReadValue<float>());
        _playerInput.PlayerMovement.ESC.performed += value => _playerController.ESCInput();
        _playerInput.PlayerMovement.WeaponSlot1.performed += value => _playerController.WeaponSlot1(0);
        _playerInput.PlayerMovement.WeaponSlot2.performed += value => _playerController.WeaponSlot1(1);
        _playerInput.PlayerMovement.WeaponSlot3.performed += value => _playerController.WeaponSlot1(2);
        _playerInput.PlayerMovement.Interact.performed += value => 
        {
            OnInteract?.Invoke();
        };

        _playerInput.Enable();
    }

    private void OnDisable()
    {
        if (_playerInput != null)
        {
            _playerInput.PlayerMovement.Movement.performed -= value => _playerController.MovementInput(value.ReadValue<Vector2>());
            _playerInput.PlayerMovement.Aim.performed -= value => _playerController.MousePosition(value.ReadValue<Vector2>());
            _playerInput.PlayerMovement.Dash.performed -= value => _playerController.Dashing();
            _playerInput.PlayerMovement.Fire.performed -= value => _playerController.WeaponShoot();
            _playerInput.PlayerMovement.HoldFire.performed -= value => _playerController.WeaponHoldShootInput(value.ReadValue<float>());
            _playerInput.PlayerMovement.ESC.performed -= value => _playerController.ESCInput();
            _playerInput.PlayerMovement.WeaponSlot1.performed -= value => _playerController.WeaponSlot1(0);
            _playerInput.PlayerMovement.WeaponSlot2.performed -= value => _playerController.WeaponSlot1(1);
            _playerInput.PlayerMovement.WeaponSlot3.performed -= value => _playerController.WeaponSlot1(2);
            _playerInput.PlayerMovement.Interact.performed -= value => OnInteract?.Invoke();

            _playerInput.Disable();
        }
    }

    private void ESC_performed(InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }
}
