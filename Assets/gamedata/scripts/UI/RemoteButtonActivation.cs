using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RemoteButtonActivation : MonoBehaviour
{
    [SerializeField] private WorldUICheck worldUICheck;
    [SerializeField] private float rayDistance = 100f;

    private Button lastSelected;

    void Update()
    {
        if (worldUICheck == null) return;

        Button button = worldUICheck.GetUIElement<Button>(rayDistance);

        if (button != lastSelected)
        {
            lastSelected = button;
            EventSystem.current.SetSelectedGameObject(button != null ? button.gameObject : null);
        }

        if (lastSelected != null && Input.GetMouseButtonDown(0))
        {
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(lastSelected.gameObject, pointer, ExecuteEvents.pointerClickHandler);
        }
    }
}
