using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameObject inventoryObj;
    [SerializeField] private InputMapSwitcher inputMapSwitcher;

    private bool isOpen = false;
    public static event Action OnSwitchInventoryEvent;

    public void OnInventorySwitch(InputAction.CallbackContext context)
    {
        if (context.performed)
            SwitchInventory();
    }

    public void SwitchInventory()
    {
        isOpen = !isOpen;
        OnSwitchInventoryEvent?.Invoke();
        inputMapSwitcher.SwitchInputMap(isOpen ? InputMapSwitcher.InputMap.Inventory : InputMapSwitcher.InputMap.Gameplay);
        inventoryObj.SetActive(isOpen);
        Debug.Log(isOpen);
    }
}
