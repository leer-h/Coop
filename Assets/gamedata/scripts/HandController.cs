using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    private ModularHand currentHand;

    [SerializeField] private GameObject[] handTools;
    [SerializeField] private Transform toolTransform;

    private int currentIndex = 0;

    private void Start()
    {
        SetTool(0);
    }

    private void SetTool(int index)
    {
        if (currentHand != null)
        {
            Destroy(currentHand.gameObject);
        }

        GameObject newHandObj = Instantiate(handTools[index], toolTransform);
        currentHand = newHandObj.GetComponent<ModularHand>();
        currentIndex = index;
    }

    public void OnSwitchHandTool(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("ToolSwitch");
            int nextIndex = (currentIndex + 1) % handTools.Length;
            SetTool(nextIndex);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            currentHand.Use();
    }
}
