using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class WorldToHudUI : MonoBehaviour
{
    [SerializeField] private WorldUICheck worldUICheck;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float rayDistance = 100f;
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private float focusDistance = 2f;
    [SerializeField] private float popUpScale = 1.2f;
    [SerializeField] private float popUpDuration = 0.2f;

    private Transform currentTarget;
    private Dictionary<Transform, UIOriginalData> originalData = new Dictionary<Transform, UIOriginalData>();

    void Update()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        if (!Input.GetMouseButton(1))
        {
            ReturnToDefault();
            return;
        }


        RectTransform rectTransform = worldUICheck.GetUIElement<RectTransform>(rayDistance);

        if (rectTransform != null && rectTransform.CompareTag("WorldToHud"))
        {
            Transform target = rectTransform.transform;

            if (currentTarget != target)
            {
                if (currentTarget != null)
                {
                    ResetTarget(currentTarget);
                }

                if (!originalData.ContainsKey(target))
                {
                    originalData[target] = new UIOriginalData(
                        target.position,
                        target.rotation,
                        target.GetSiblingIndex(),
                        target.localScale
                    );
                }

                target.SetAsLastSibling();

                Crosshair.Hide();
                target.GetComponentInParent<Canvas>().gameObject.layer = LayerMask.NameToLayer("WorldUI");

                Vector3 targetPos = playerCamera.transform.position + playerCamera.transform.forward * focusDistance;
                target.DOMove(targetPos, duration);

                target.DOScale(originalData[target].scale * popUpScale, popUpDuration)
                        .SetLoops(2, LoopType.Yoyo);

                currentTarget = target;
            }

            Quaternion lookRot;
            FlipMarker marker = currentTarget.GetComponent<FlipMarker>();

            if (marker != null && marker.flipRotation)
            {
                lookRot = Quaternion.LookRotation(currentTarget.position - playerCamera.transform.position);
            }
            else
            {
                lookRot = Quaternion.LookRotation(playerCamera.transform.position - currentTarget.position);
            }

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
            currentTarget.GetComponentInParent<Canvas>().gameObject.layer = LayerMask.NameToLayer("UI");

            ResetTarget(currentTarget);
            currentTarget = null;
        }
    }

    private void ResetTarget(Transform target)
    {
        if (originalData.ContainsKey(target))
        {
            UIOriginalData data = originalData[target];
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
}
