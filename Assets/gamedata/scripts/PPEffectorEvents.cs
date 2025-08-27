using System;
using UnityEngine;

public class PPEffectorEvents : MonoBehaviour
{
    [SerializeField] private PPEffector ppEffector;

    private void OnEnable()
    {
        WaterTrigger.OnWaterEnter += HandleWaterEnter;
        WaterTrigger.OnWaterExit += HandleWaterExit;
    }

    private void OnDisable()
    {
        WaterTrigger.OnWaterEnter -= HandleWaterEnter;
        WaterTrigger.OnWaterExit -= HandleWaterExit;
    }

    private void HandleWaterEnter()
    {
        ppEffector.SetEffect(ppEffector.Chromatic, ppEffector.Chromatic.intensity, 1f, 1f);
        ppEffector.SetEffect(ppEffector.Panini, ppEffector.Panini.distance, 0.3f, 1f);
    }

    private void HandleWaterExit()
    {
        ppEffector.SetEffect(ppEffector.Chromatic, ppEffector.Chromatic.intensity, 0f, 1f);
        ppEffector.SetEffect(ppEffector.Panini, ppEffector.Panini.distance, 0f, 1f);
    }
}
