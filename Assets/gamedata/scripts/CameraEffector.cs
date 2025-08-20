using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;
using System.Collections.Generic;

public class CameraEffects : MonoBehaviour
{
    private Quaternion baseRotation;
    private Quaternion currentAnimRotation = Quaternion.identity;
    private Coroutine animCoroutine;

    private Dictionary<string, AnimationClip> clipCache = new Dictionary<string, AnimationClip>();

    private GameObject tempCam;

    private void Awake()
    {
        baseRotation = transform.localRotation;

        tempCam = new GameObject("TempCam");
        tempCam.AddComponent<Animator>();
        tempCam.transform.localRotation = Quaternion.identity;
        tempCam.hideFlags = HideFlags.HideAndDontSave;
    }

    private void OnDestroy()
    {
        if (tempCam != null)
            Destroy(tempCam);
    }

    private void OnDisable()
    {
        ClearCache();
    }

    private void Update()
    {
        baseRotation = transform.localRotation;
        transform.localRotation = baseRotation * currentAnimRotation;
    }

    public void AddCamEffector(string effectName, float animationMultiplier = 1f, float speed = 1f)
    {
        if (clipCache.TryGetValue(effectName, out var cachedClip))
        {
            PlayCachedClip(cachedClip, animationMultiplier, speed);
            return;
        }

        LoadClip(effectName, (clip) =>
        {
            if (clip == null) return;

            clipCache[effectName] = clip;
            PlayCachedClip(clip, animationMultiplier, speed);
        });
    }

    private void PlayCachedClip(AnimationClip clip, float animationMultiplier, float speed)
    {
        if (animCoroutine != null)
            StopCoroutine(animCoroutine);

        animCoroutine = StartCoroutine(PlayClipAdditive(clip, animationMultiplier, speed));
    }

    private void LoadClip(string effectName, System.Action<AnimationClip> onLoaded)
    {
        Addressables.LoadAssetAsync<AnimationClip>(effectName).Completed += (AsyncOperationHandle<AnimationClip> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
                onLoaded?.Invoke(handle.Result);
            else
            {
                Debug.LogError($"CameraEffects: can't load anim {effectName}");
                onLoaded?.Invoke(null);
            }
        };
    }

    private IEnumerator PlayClipAdditive(AnimationClip clip, float animationMultiplier, float speed)
    {
        float timer = 0f;
        float fadeDuration = 0.1f;

        while (timer < clip.length)
        {
            clip.SampleAnimation(tempCam, timer);

            Quaternion targetRotation = tempCam.transform.localRotation;
            targetRotation.x *= -1;

            Vector3 eulerAngles = targetRotation.eulerAngles;
            eulerAngles *= animationMultiplier;
            targetRotation = Quaternion.Euler(eulerAngles);

            float t = Mathf.Clamp01(Time.deltaTime / fadeDuration);
            currentAnimRotation = Quaternion.Slerp(currentAnimRotation, targetRotation, t);

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

    private void ClearCache()
    {
        foreach (var clip in clipCache.Values)
            Addressables.Release(clip);
        clipCache.Clear();
    }
}
