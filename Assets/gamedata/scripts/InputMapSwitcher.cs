using UnityEngine;
using UnityEngine.InputSystem;

public class InputMapSwitcher : MonoBehaviour
{
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void SwitchToUI()
    {
        playerInput.SwitchCurrentActionMap("WorldUI");
        Debug.Log("Input Switched to UI");
    }

    public void SwitchToGameplay()
    {
        playerInput.SwitchCurrentActionMap("Gameplay");
        Debug.Log("Input Switched to Gameplay");
    }
}
