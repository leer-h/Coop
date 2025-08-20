using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Photon.Pun;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;
    private CanvasGroup fadeCanvas;
    private UnityEngine.UI.Image fadeImage;
    private const float fadeDuration = 0.7f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        GameObject canvasObj = new GameObject("FadeCanvas");
        canvasObj.transform.SetParent(transform);

        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999;

        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        fadeCanvas = canvasObj.AddComponent<CanvasGroup>();
        fadeCanvas.alpha = 0;

        RectTransform rt = canvasObj.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        GameObject imgObj = new GameObject("FadeImage");
        imgObj.transform.SetParent(canvasObj.transform, false);

        fadeImage = imgObj.AddComponent<UnityEngine.UI.Image>();
        fadeImage.color = Color.black;
        fadeImage.gameObject.SetActive(false);

        RectTransform imgRT = imgObj.GetComponent<RectTransform>();
        imgRT.anchorMin = Vector2.zero;
        imgRT.anchorMax = Vector2.one;
        imgRT.offsetMin = Vector2.zero;
        imgRT.offsetMax = Vector2.zero;

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    public void LoadSceneWithFade(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);
        fadeCanvas.DOFade(1, fadeDuration).OnComplete(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }

    public void LoadPhotonSceneWithFade(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);
        fadeCanvas.DOFade(1, fadeDuration).OnComplete(() =>
        {
            PhotonNetwork.LoadLevel(sceneName);
        });
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fadeImage.gameObject.SetActive(true);
        fadeCanvas.alpha = 1;
        fadeCanvas.DOFade(0, fadeDuration)
            .SetAutoKill(true)
            .OnComplete(() =>
            {
                fadeImage.gameObject.SetActive(false);
            });
    }

    private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        Debug.Log($"Active scene changed: {oldScene.name} -> {newScene.name}");
    }
}
