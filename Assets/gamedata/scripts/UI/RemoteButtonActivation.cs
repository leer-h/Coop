using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class RemoteButtonActivation : MonoBehaviour
{
    [SerializeField] private WorldUICheck worldUICheck;
    [SerializeField] private float rayDistance = 100f;

    private Button lastSelected;
    private Tween currentTween;
    private Vector3 originalScale;

    void Update()
    {
        if (worldUICheck == null) return;

        Button button = worldUICheck.GetButton(rayDistance);

        if (button != lastSelected)
        {
            if (currentTween != null && currentTween.IsActive())
            {
                currentTween.Kill();
                if (lastSelected != null)
                    lastSelected.transform.localScale = originalScale;
            }

            lastSelected = button;
            EventSystem.current.SetSelectedGameObject(button != null ? button.gameObject : null);

            if (button != null)
            {
                originalScale = button.transform.localScale;

                currentTween = button.transform.DOPunchScale(
                    new Vector2(0.5f, 0.5f),
                    0.5f,
                    10,
                    1f
                );
            }
        }

        if (lastSelected != null && Input.GetMouseButtonDown(0))
        {
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(lastSelected.gameObject, pointer, ExecuteEvents.pointerClickHandler);
        }
    }
}
