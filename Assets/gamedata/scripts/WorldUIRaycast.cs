using UnityEngine;
using UnityEngine.UI;

public class WorldUICheck : MonoBehaviour
{
    private int worldUILayer;

    void Start()
    {
        worldUILayer = LayerMask.NameToLayer("WorldUI");
    }

    public Button GetButton(float rayDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        int layerMask = 1 << worldUILayer;

        if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
        {
            Button button = hit.collider.GetComponent<Button>();
            if (button != null)
            {
                return button;
            }
        }

        return null;
    }
}
