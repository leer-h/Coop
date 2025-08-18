using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RemoteButtonActivation : MonoBehaviour
{
    [SerializeField] private WorldUICheck worldUICheck;
    [SerializeField] private float rayDistance = 100f;

    private Button lastSelected;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && lastSelected != null)
        {
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(lastSelected.gameObject, pointer, ExecuteEvents.pointerClickHandler);
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
        }
    }
}
