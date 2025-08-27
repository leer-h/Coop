using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class PPEffector : MonoBehaviour
{
    [SerializeField] private Volume volume;

    private ChromaticAberration chromatic;
    private PaniniProjection panini;

    public ChromaticAberration Chromatic => chromatic;
    public PaniniProjection Panini => panini;

    void Awake()
    {
        if (volume != null && volume.profile != null)
        {
            volume.profile.TryGet(out chromatic);
            volume.profile.TryGet(out panini);
        }
    }

    public Tween SetEffect(VolumeComponent component, FloatParameter parameter, float targetValue, float duration = 1f)
    {
        if (component == null || parameter == null) return null;

        component.active = true;

        return DOTween.To(
            () => parameter.value,
            x => parameter.value = x,
            targetValue,
            duration
        );
    }
}
