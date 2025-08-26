using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputMapSwitcher : MonoBehaviour
{
    private PlayerInput playerInput;
    public static event Action<string> OnInputMapSwitch;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void SwitchToUI()
    {
        string mapName = "WorldUI";
        playerInput.SwitchCurrentActionMap(mapName);
        Debug.Log($"Input Switched to {mapName}");

        OnInputMapSwitch?.Invoke(mapName);
    }

    public void SwitchToGameplay()
    {
        string mapName = "Gameplay";
        playerInput.SwitchCurrentActionMap(mapName);
        Debug.Log($"Input Switched to {mapName}");

        OnInputMapSwitch?.Invoke(mapName);
    }
}
