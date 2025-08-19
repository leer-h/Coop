using UnityEngine;

public class CameraEffectsEvents : MonoBehaviour
{
    private void OnEnable()
    {
        CharacterMovement.OnJumpEvent += HandleJump;
        CharacterMovement.OnLandEvent += HandleLand;
        CharacterMovement.OnCrouchEvent += HandleCrouch;
        CharacterMovement.OnMovingEvent += HandleMove;
    }

    private void OnDisable()
    {
        CharacterMovement.OnJumpEvent -= HandleJump;
        CharacterMovement.OnLandEvent -= HandleLand;
        CharacterMovement.OnCrouchEvent -= HandleCrouch;
        CharacterMovement.OnMovingEvent -= HandleMove;
    }

    private void HandleJump()
    {
        CameraEffects.AddCamEffector("cam_jump", 3f);
    }

    private void HandleLand()
    {
        CameraEffects.AddCamEffector("cam_land", 3f, 2f);
    }

    private void HandleCrouch(bool isCrouching)
    {
        if (isCrouching)
            CameraEffects.AddCamEffector("cam_crouch", 3f);
        else
            CameraEffects.AddCamEffector("cam_stand", 3f);
    }

    private void HandleMove(Vector2 moveInput)
    {
        CameraEffects.AddCamEffector("cam_move", 15f);
    }
}
