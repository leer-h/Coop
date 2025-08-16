using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class WorldUIFocusMover : MonoBehaviour
{
    [SerializeField] private WorldUICheck worldUICheck;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float rayDistance = 100f;
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private float focusDistance = 2f;

    private Transform currentTarget;
    private Dictionary<Transform, UIOriginalData> originalData = new Dictionary<Transform, UIOriginalData>();

    void Update()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

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
                    originalData[target] = new UIOriginalData(target.position, target.rotation, target.GetSiblingIndex());
                }

                target.SetAsLastSibling();

                Vector3 targetPos = playerCamera.transform.position + playerCamera.transform.forward * focusDistance;
                target.DOMove(targetPos, duration);

                currentTarget = target;
            }

            currentTarget.rotation = Quaternion.LookRotation(playerCamera.transform.position - currentTarget.position);
        }
        else
        {
            if (currentTarget != null)
            {
                ResetTarget(currentTarget);
                currentTarget = null;
            }
        }
    }

    private void ResetTarget(Transform target)
    {
        if (originalData.ContainsKey(target))
        {
            UIOriginalData data = originalData[target];
            target.DOMove(data.position, duration);
            target.rotation = data.rotation;
            target.SetSiblingIndex(data.siblingIndex);
        }
    }

    private class UIOriginalData
    {
        public Vector3 position;
        public Quaternion rotation;
        public int siblingIndex;

        public UIOriginalData(Vector3 pos, Quaternion rot, int index)
        {
            position = pos;
            rotation = rot;
            siblingIndex = index;
        }
    }
}
