using System;
using UnityEngine;

public class CameraEffectsEvents : MonoBehaviour
{
    [SerializeField] private CameraEffects cameraEffects;

    private void OnEnable()
    {
        CharacterMovement.OnJumpEvent += HandleJump;
        CharacterMovement.OnLandEvent += HandleLand;
        CharacterMovement.OnCrouchEvent += HandleCrouch;
        CharacterMovement.OnMovingEvent += HandleMove;

        PlayerInventory.OnSwitchInventoryEvent += HandleInventotySwitch;
    }

    private void OnDisable()
    {
        CharacterMovement.OnJumpEvent -= HandleJump;
        CharacterMovement.OnLandEvent -= HandleLand;
        CharacterMovement.OnCrouchEvent -= HandleCrouch;
        CharacterMovement.OnMovingEvent -= HandleMove;

        PlayerInventory.OnSwitchInventoryEvent -= HandleInventotySwitch;
    }

    private void HandleJump()
    {
        cameraEffects.AddCamEffector("cam_jump", 3f);
    }

    private void HandleLand()
    {
        cameraEffects.AddCamEffector("cam_land", 3f, 2f);
    }

    private void HandleCrouch(bool isCrouching)
    {
        if (isCrouching)
            cameraEffects.AddCamEffector("cam_crouch", 3f);
        else
            cameraEffects.AddCamEffector("cam_stand", 3f);
    }

    private void HandleMove(Vector2 moveInput)
    {
        cameraEffects.AddCamEffector("cam_move", 15f);
    }

    private void HandleInventotySwitch()
    {
        cameraEffects.AddCamEffector("cam_inv_switch", 10f,2f);
    }
}
