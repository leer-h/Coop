using UnityEngine;

public class WorldUICheck : MonoBehaviour
{
    private int worldUILayer;

    void Start()
    {
        worldUILayer = LayerMask.NameToLayer("WorldUI");
    }

    public T GetUIElement<T>(float rayDistance) where T : Component
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        int layerMask = 1 << worldUILayer;

        if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
        {
            return hit.collider.GetComponent<T>();
        }

        return null;
    }
}
