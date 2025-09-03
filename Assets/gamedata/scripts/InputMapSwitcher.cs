using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputMapSwitcher : MonoBehaviour
{
    private PlayerInput playerInput;
    public static event Action<string> OnInputMapSwitch;

    public enum InputMap
    {
        Gameplay,
        WorldUI,
        Inventory
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void SwitchInputMap(InputMap map)
    {
        string mapName = map.ToString();
        playerInput.SwitchCurrentActionMap(mapName);
        Debug.Log($"Input switched to {mapName}");

        OnInputMapSwitch?.Invoke(mapName);
    }
}
