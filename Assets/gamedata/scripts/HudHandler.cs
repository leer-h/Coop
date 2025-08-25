using UnityEngine;
using System.Collections;

public class HudHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Follow Settings")]
    [SerializeField] private Vector3 positionOffset = new(0.2f, -0.2f, 0.5f);
    [SerializeField] private Vector3 rotationOffset = Vector3.zero;
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private float rotateSpeed = 10f;

    [Header("Sway Settings")]
    [SerializeField] private float swayAmount = 0.05f;
    [SerializeField] private float swayRotation = 2f;
    [SerializeField] private float swaySmooth = 6f;

    [Header("Hand Animation Settings")]
    [SerializeField] private Vector3 jumpOffset = new(0, 0.15f, 0.1f);
    [SerializeField] private Vector3 landOffset = new(0, -0.1f, -0.05f);
    [SerializeField] private Vector3 crouchOffset = new(0, -0.15f, 0);
    [SerializeField] private Vector3 standOffset = Vector3.zero;
    [SerializeField] private float moveOffsetMultiplier = 0.05f;

    [SerializeField] private Vector3 jumpRotation = new(-30, 0, 0);
    [SerializeField] private Vector3 landRotation = new(20, 0, 0);
    [SerializeField] private Vector3 crouchRotation = new(40, 0, 0);
    [SerializeField] private Vector3 standRotation = Vector3.zero;

    [SerializeField] private float jumpDuration = 0.15f;
    [SerializeField] private float landDuration = 0.12f;
    [SerializeField] private float crouchDuration = 0.18f;
    [SerializeField] private float standDuration = 0.18f;
    [SerializeField] private float moveDuration = 0.1f;
    [SerializeField] private float returnDuration = 0.18f;
    [SerializeField] private float returnDelay = 0.08f;

    private Vector3 targetPos;
    private Quaternion targetRot;
    private Vector3 swayPosOffset;
    private Quaternion swayRotOffset;

    private Vector3 handAnimOffset = Vector3.zero;
    private Quaternion handAnimRotation = Quaternion.identity;
    private Coroutine handAnimCoroutine;

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
        PlayHandJump();
    }

    private void HandleLand()
    {
        PlayHandLand();
    }

    private void HandleCrouch(bool isCrouching)
    {
        PlayHandCrouch(isCrouching);
    }

    private void HandleMove(Vector2 moveInput)
    {
        PlayHandMove(moveInput);
    }

    private void PlayHandJump()
    {
        StartHandAnim(jumpOffset, Quaternion.Euler(jumpRotation), jumpDuration);
    }

    private void PlayHandLand()
    {
        StartHandAnim(landOffset, Quaternion.Euler(landRotation), landDuration);
    }

    private void PlayHandCrouch(bool isCrouching)
    {
        if (isCrouching)
            StartHandAnim(crouchOffset, Quaternion.Euler(crouchRotation), crouchDuration);
        else
            StartHandAnim(standOffset, Quaternion.Euler(standRotation), standDuration);
    }

    private void PlayHandMove(Vector2 moveInput)
    {
        float moveAmount = Mathf.Clamp(moveInput.magnitude, 0, 1);
        StartHandAnim(new Vector3(0, 0, moveAmount * moveOffsetMultiplier), Quaternion.identity, moveDuration);
    }

    private void StartHandAnim(Vector3 targetOffset, Quaternion targetRot, float duration)
    {
        if (handAnimCoroutine != null)
            StopCoroutine(handAnimCoroutine);
        handAnimCoroutine = StartCoroutine(HandAnimRoutine(targetOffset, targetRot, duration));
    }

    private IEnumerator HandAnimRoutine(Vector3 targetOffset, Quaternion targetRot, float duration)
    {
        Vector3 startOffset = handAnimOffset;
        Quaternion startRot = handAnimRotation;
        float timer = 0f;
        while (timer < duration)
        {
            float t = timer / duration;
            handAnimOffset = Vector3.Lerp(startOffset, targetOffset, t);
            handAnimRotation = Quaternion.Slerp(startRot, targetRot, t);
            timer += Time.deltaTime;
            yield return null;
        }
        handAnimOffset = targetOffset;
        handAnimRotation = targetRot;

        yield return new WaitForSeconds(returnDelay);
        timer = 0f;
        Vector3 baseOffset = Vector3.zero;
        Quaternion baseRot = Quaternion.identity;
        Vector3 fromOffset = handAnimOffset;
        Quaternion fromRot = handAnimRotation;
        while (timer < returnDuration)
        {
            float t = timer / returnDuration;
            handAnimOffset = Vector3.Lerp(fromOffset, baseOffset, t);
            handAnimRotation = Quaternion.Slerp(fromRot, baseRot, t);
            timer += Time.deltaTime;
            yield return null;
        }
        handAnimOffset = baseOffset;
        handAnimRotation = baseRot;
        handAnimCoroutine = null;
    }

    void LateUpdate()
    {
        targetPos = cameraTransform.position + cameraTransform.TransformDirection(positionOffset);
        targetRot = cameraTransform.rotation * Quaternion.Euler(rotationOffset);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 targetSwayPos = new Vector3(-mouseX, -mouseY, 0) * swayAmount/100;
        swayPosOffset = Vector3.Lerp(swayPosOffset, targetSwayPos, Time.deltaTime * swaySmooth);

        Vector3 targetSwayRot = new Vector3(mouseY, -mouseX, 0) * swayRotation;
        swayRotOffset = Quaternion.Slerp(swayRotOffset, Quaternion.Euler(targetSwayRot), Time.deltaTime * swaySmooth);

        transform.localPosition += handAnimOffset;
        transform.localRotation *= handAnimRotation;

        transform.localPosition += swayPosOffset;
        transform.localRotation *= swayRotOffset;
    }
}
