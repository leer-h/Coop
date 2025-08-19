using UnityEngine;
using System.Collections;

public class CameraEffects : MonoBehaviour
{
    private static CameraEffects instance;
    private Quaternion baseRotation;

    private Quaternion currentAnimRotation = Quaternion.identity;
    private Coroutine animCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            baseRotation = transform.localRotation;
        }
    }

    private void Update()
    {
        baseRotation = transform.localRotation;
        transform.localRotation = baseRotation * currentAnimRotation;
    }

    public static void AddCamEffector(string effectName, float animationMultiplier = 1f, float speed = 1f)
    {
        if (instance != null)
        {
            AnimationClip clip = instance.LoadClip(effectName);
            if (clip == null) return;

            if (instance.animCoroutine != null)
                instance.StopCoroutine(instance.animCoroutine);

            instance.animCoroutine = instance.StartCoroutine(instance.PlayClipAdditive(clip, animationMultiplier, speed));
        }
        else
        {
            Debug.LogError("CameraEffects instance not found!");
        }
    }

    private AnimationClip LoadClip(string effectName)
    {
        string path = $"cam_anims/{effectName}";
        AnimationClip clip = Resources.Load<AnimationClip>(path);
        if (clip == null)
        {
            Debug.LogError($"CameraEffects: can't find anim in {path}");
        }
        return clip;
    }

    private IEnumerator PlayClipAdditive(AnimationClip clip, float animationMultiplier, float speed)
    {
        float timer = 0f;
        float fadeDuration = 0.1f;

        while (timer < clip.length)
        {
            GameObject temp = new GameObject("TempCam");
            temp.transform.localRotation = Quaternion.identity;

            clip.SampleAnimation(temp, timer);

            Quaternion targetRotation = temp.transform.localRotation;
            targetRotation.x *= -1;

            Vector3 eulerAngles = targetRotation.eulerAngles;
            eulerAngles *= animationMultiplier;
            targetRotation = Quaternion.Euler(eulerAngles);

            float t = Mathf.Clamp01(Time.deltaTime / fadeDuration);
            currentAnimRotation = Quaternion.Slerp(currentAnimRotation, targetRotation, t);

            Destroy(temp);

            timer += Time.deltaTime * speed;
            yield return null;
        }

        float fadeOut = 0f;
        while (fadeOut < 1f)
        {
            currentAnimRotation = Quaternion.Slerp(currentAnimRotation, Quaternion.identity, fadeOut);
            fadeOut += Time.deltaTime / fadeDuration;
            yield return null;
        }

        currentAnimRotation = Quaternion.identity;
        animCoroutine = null;
    }
}
