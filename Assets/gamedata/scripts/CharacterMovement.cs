using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using System;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PlayerInput))]
public class CharacterMovement : MonoBehaviourPun
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float moveEventInterval = 0.5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float transitionSpeed = 5f;

    [Header("Mouse Look")]
    [SerializeField] private Transform playerCamera;
    private float xRotation = 0f;
    private float targetCameraHeight;
    private float currentCameraHeight;

    private float moveEventTimer = 0f;
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching;
    private bool wasGrounded;
    private bool isMovementLocked = false;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpPressed;
    private bool crouchPressed;

    public static event Action OnJumpEvent;
    public static event Action OnLandEvent;
    public static event Action<Vector2> OnMovingEvent;
    public static event Action<bool> OnCrouchEvent;

    private void OnEnable()
    {
        CameraEffects.OnManualCamOn += DisableMovement;
        CameraEffects.OnManualCamOff += EnableMovement;
    }

    private void OnDisable()
    {
        CameraEffects.OnManualCamOn -= DisableMovement;
        CameraEffects.OnManualCamOff -= EnableMovement;
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        if (!photonView.IsMine)
        {
            if (playerCamera != null)
                playerCamera.gameObject.SetActive(false);
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentCameraHeight = standHeight * 0.6f;
        targetCameraHeight = currentCameraHeight;
        UpdateCameraPosition();
    }

    private void Update()
    {
        if (!photonView.IsMine || isMovementLocked) return;

        isGrounded = controller.isGrounded;

        if (!wasGrounded && isGrounded)
        {
            OnLand();
        }
        wasGrounded = isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        HandleMouseLook();
        HandleMovement();
        HandleJump();
        HandleCrouch();

        currentCameraHeight = Mathf.Lerp(currentCameraHeight, targetCameraHeight, Time.deltaTime * transitionSpeed);
        UpdateCameraPosition();

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    #region Input Callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) jumpPressed = true;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
            crouchPressed = true;
        if (context.canceled)
            crouchPressed = false;
    }
    #endregion



    private void HandleMouseLook()
    {
        float mouseX = lookInput.x;
        float mouseY = lookInput.y;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        float speed = isCrouching ? crouchSpeed : walkSpeed;

        controller.Move(speed * Time.deltaTime * move);

        if (moveInput.sqrMagnitude > 0f)
        {
            moveEventTimer += Time.deltaTime;
            if (moveEventTimer >= moveEventInterval)
            {
                OnMovingEvent?.Invoke(moveInput);
                moveEventTimer = 0f;
            }
        }
        else
        {
            moveEventTimer = moveEventInterval;
        }
    }

    private void HandleJump()
    {
        if (jumpPressed && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            OnJumpEvent?.Invoke();
        }
        jumpPressed = false;
    }

    private void HandleCrouch()
    {
        if (crouchPressed)
        {
            if (!isCrouching)
                OnCrouchEvent?.Invoke(true);

            isCrouching = true;
            targetCameraHeight = crouchHeight * 0.6f;
        }
        else
        {
            if (isCrouching)
                OnCrouchEvent?.Invoke(false);

            isCrouching = false;
            targetCameraHeight = standHeight * 0.6f;
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 cameraPos = playerCamera.localPosition;
        cameraPos.y = currentCameraHeight;
        playerCamera.localPosition = cameraPos;
    }

    private void OnLand()
    {
        OnLandEvent?.Invoke();
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    private void DisableMovement()
    {
        isMovementLocked = true;
    }

    private void EnableMovement()
    {
        isMovementLocked = false;
    }

    public Vector2 GetMoveInput() => moveInput;
}