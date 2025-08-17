using UnityEngine;
using DG.Tweening;

public class WorldUICheck : MonoBehaviour
{
    private int worldUILayer;

    private Transform lastTarget;
    private Tween currentTween;
    private Vector3 originalScale;

    [Header("Анімація скейлу")]
    [SerializeField] private Vector3 punchStrength = new Vector3(0.5f, 0.5f, 0f);
    [SerializeField] private float punchDuration = 0.5f;
    [SerializeField] private int vibrato = 10;
    [SerializeField] private float elasticity = 1f;

    void Start()
    {
        worldUILayer = LayerMask.NameToLayer("WorldUI");
    }

    public T GetUIElement<T>(float rayDistance) where T : Component
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        int layerMask = 1 << worldUILayer;

        if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
        {
            HandleScaleAnimation(hit.collider.transform);

            return hit.collider.GetComponent<T>();
        }

        ResetLastTarget();
        return null;
    }

    private void HandleScaleAnimation(Transform target)
    {
        if (target == lastTarget) return;

        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
            if (lastTarget != null)
                lastTarget.localScale = originalScale;
        }

        lastTarget = target;
        originalScale = target.localScale;

        currentTween = target.DOPunchScale(punchStrength, punchDuration, vibrato, elasticity);
    }

    private void ResetLastTarget()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
            if (lastTarget != null)
                lastTarget.localScale = originalScale;
        }

        lastTarget = null;
    }
}
