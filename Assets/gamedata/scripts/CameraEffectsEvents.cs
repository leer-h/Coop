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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            var pos = new Vector3(4.555f, 2.5f, 2.77f);
            var posLook = new Vector3(7.99f, 2.5f, 2.77f);
            cameraEffects.MoveAndLookAt(pos, posLook, 1f);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            cameraEffects.ResetCamPos(1f);
        }
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


}
