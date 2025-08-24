using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;

public class RemoteButtonActivation : MonoBehaviour
{
    [SerializeField] private WorldUICheck worldUICheck;
    [SerializeField] private float rayDistance = 100f;
    [SerializeField] private InputMapSwitcher inputMapSwitcher;

    private Button lastSelected;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && lastSelected != null)
        {
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(lastSelected.gameObject, pointer, ExecuteEvents.pointerClickHandler);

            lastSelected.transform.DOJump(
                lastSelected.transform.position + lastSelected.transform.forward * 0.1f,
                0.1f,
                1,
                0.2f
            ).SetEase(Ease.OutQuad).OnComplete(() =>
                lastSelected.transform.DOJump(
                    lastSelected.transform.position - lastSelected.transform.forward * 0.1f,
                    0.1f,
                    1,
                    0.2f
                ).SetEase(Ease.InQuad)
            );
        }
    }

    private void Update()
    {
        if (worldUICheck == null) return;

        Button button = worldUICheck.GetUIElement<Button>(rayDistance);

        if (button != lastSelected)
        {
            lastSelected = button;
            EventSystem.current.SetSelectedGameObject(button != null ? button.gameObject : null);

            if (button != null)
                inputMapSwitcher.SwitchToUI();
            else
                inputMapSwitcher.SwitchToGameplay();
        }
    }

}
