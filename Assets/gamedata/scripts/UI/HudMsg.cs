using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudMsg : MonoBehaviour
{
    public static HudMsg Instance { get; private set; }

    [SerializeField] private Text _txt;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public static void SetHudMsg(string str, Vector3 pos, int size, bool useAnm, float secondToRemove)
    {
        if (Instance == null)
        {
            Debug.LogWarning("HudMsg: Instance is null!");
            return;
        }

        var _txt = Instance._txt;

        if (useAnm)
        {
            _txt.color = new Color(1, 1, 1, 0);
            Instance.StartCoroutine(Instance.LerpAnm(secondToRemove));
        }
        else
        {
            _txt.color = new Color(1, 1, 1, 1);
            Instance.Invoke(nameof(TextRemove), secondToRemove);
        }

        _txt.transform.localPosition = pos;
        _txt.text = str;
        _txt.fontSize = size;
    }

    private IEnumerator LerpAnm(float secondToRemove)
    {
        Color targetColor = _txt.color;
        int targetAlphaVal = 1;
        for (int i = 0; i < 2; i++)
        {
            while (_txt.color.a != targetAlphaVal)
            {
                var c = Mathf.MoveTowards(_txt.color.a, targetAlphaVal, 5f * Time.deltaTime);
                targetColor.a = c;
                _txt.color = targetColor;
                yield return null;
            }
            targetAlphaVal = 0;
            yield return new WaitForSeconds(secondToRemove);
        }
        Invoke(nameof(TextRemove), 2);
    }

    private void TextRemove()
    {
        _txt.text = null;
    }
}
