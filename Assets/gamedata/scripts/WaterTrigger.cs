using System;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    public static Action OnWaterEnter;
    public static Action OnWaterExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnWaterEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnWaterExit?.Invoke();
        }
    }
}
