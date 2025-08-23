using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class WorldToHudUI : MonoBehaviour
{
    [SerializeField] private WorldUICheck worldUICheck;
    [SerializeField] private Camera playerCamera;

    [Header("Raycast Settings")]
    [SerializeField] private float rayDistance = 100f;
    [SerializeField] private float focusDistance = 2f;

    [Header("PopUp Animation Settings")]
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private float popUpScale = 1.2f;
    [SerializeField] private float popUpDuration = 0.2f;

    [Header("Punch Animation Settings")]
    [SerializeField] private Vector3 punchStrength = new (0.5f, 0.5f, 0f);
    [SerializeField] private float punchDuration = 0.5f;
    [SerializeField] private int vibrato = 10;
    [SerializeField] private float elasticity = 1f;

    private Transform currentTarget;
    private readonly Dictionary<Transform, UIOriginalData> originalData = new();
    private bool isFocusing;

    private Tween scaleTween;
    private Tween punchTween;
    private Vector3 punchOriginalScale;
    private Transform punchLastTarget;

    void Update()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        CheckForPunch();

        if (!isFocusing)
        {
            ReturnToDefault();
            return;
        }

        HandleFocusUI();
    }

    private void CheckForPunch()
    {
        RectTransform rectTransform = worldUICheck.GetUIElement<RectTransform>(rayDistance);
        if (rectTransform != null)
            HandlePunchScaleAnimation(rectTransform.transform);
        else
            ResetPunchLastTarget();
    }

    private void HandlePunchScaleAnimation(Transform target)
    {
        if (target == punchLastTarget) return;

        if (punchTween != null && punchTween.IsActive())
        {
            punchTween.Kill();
            if (punchLastTarget != null)
                punchLastTarget.localScale = punchOriginalScale;
        }

        punchLastTarget = target;
        punchOriginalScale = target.localScale;

        punchTween = target.DOPunchScale(punchStrength, punchDuration, vibrato, elasticity);
    }

    private void ResetPunchLastTarget()
    {
        if (punchTween != null && punchTween.IsActive())
        {
            punchTween.Kill();
            if (punchLastTarget != null)
                punchLastTarget.localScale = punchOriginalScale;
        }
        punchLastTarget = null;
    }

    private void HandleFocusUI()
    {
        RectTransform rectTransform = worldUICheck.GetUIElement<RectTransform>(rayDistance);

        if (rectTransform != null && rectTransform.CompareTag("WorldToHud"))
        {
            Transform target = rectTransform.transform;

            if (currentTarget != target)
            {
                if (currentTarget != null)
                    ResetTarget(currentTarget, true);

                if (!originalData.ContainsKey(target))
                {
                    Vector3 originalScaleToStore = (target == punchLastTarget) ? punchOriginalScale : target.localScale;
                    originalData[target] = new UIOriginalData(
                        target.position,
                        target.rotation,
                        target.GetSiblingIndex(),
                        originalScaleToStore
                    );
                }

                target.SetAsLastSibling();
                Crosshair.Hide();
                target.GetComponentInParent<Canvas>().gameObject.layer = LayerMask.NameToLayer("WorldUI");

                Vector3 targetPos = playerCamera.transform.position + playerCamera.transform.forward * focusDistance;
                target.DOMove(targetPos, duration);

                if (scaleTween != null && scaleTween.IsActive()) scaleTween.Kill();
                scaleTween = target.DOScale(originalData[target].scale * popUpScale, popUpDuration)
                                   .SetLoops(2, LoopType.Yoyo);

                currentTarget = target;
            }

            Quaternion lookRot;
            FlipMarker marker = currentTarget.GetComponent<FlipMarker>();
            if (marker != null && marker.flipRotation)
                lookRot = Quaternion.LookRotation(currentTarget.position - playerCamera.transform.position);
            else
                lookRot = Quaternion.LookRotation(playerCamera.transform.position - currentTarget.position);

            currentTarget.rotation = lookRot;
        }
        else
        {
            ReturnToDefault();
        }
    }

    private void ReturnToDefault()
    {
        Crosshair.Show();

        if (currentTarget != null)
        {
            ResetTarget(currentTarget, true);
            currentTarget = null;
        }
    }

    private void ResetTarget(Transform target, bool killTween)
    {
        if (originalData.ContainsKey(target))
        {
            UIOriginalData data = originalData[target];

            if (killTween && scaleTween != null && scaleTween.IsActive()) scaleTween.Kill();

            target.DOMove(data.position, duration);
            target.DORotateQuaternion(data.rotation, duration);
            target.DOScale(data.scale, popUpDuration);
            target.SetSiblingIndex(data.siblingIndex);
        }
    }

    private class UIOriginalData
    {
        public Vector3 position;
        public Quaternion rotation;
        public int siblingIndex;
        public Vector3 scale;

        public UIOriginalData(Vector3 pos, Quaternion rot, int index, Vector3 scl)
        {
            position = pos;
            rotation = rot;
            siblingIndex = index;
            scale = scl;
        }
    }

    public void OnFocusUI(InputAction.CallbackContext context)
    {
        isFocusing = context.ReadValue<float>() > 0f;
    }
}
