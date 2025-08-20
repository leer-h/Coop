using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HudMsg : MonoBehaviour
{
    [SerializeField] private Text _txt;

    public void SetHudMsg(string str, Vector3 pos, int size, bool useAnm, float secondToRemove)
    {
        if (_txt == null) return;

        if (useAnm)
        {
            _txt.color = new Color(1, 1, 1, 0);
            StartCoroutine(LerpAnm(secondToRemove));
        }
        else
        {
            _txt.color = new Color(1, 1, 1, 1);
            Invoke(nameof(TextRemove), secondToRemove);
        }

        _txt.transform.localPosition = pos;
        _txt.text = str;
        _txt.fontSize = size;
    }

    private IEnumerator LerpAnm(float secondToRemove)
    {
        Color targetColor = _txt.color;
        float targetAlpha = 1f;
        for (int i = 0; i < 2; i++)
        {
            while (!Mathf.Approximately(_txt.color.a, targetAlpha))
            {
                targetColor.a = Mathf.MoveTowards(_txt.color.a, targetAlpha, 5f * Time.deltaTime);
                _txt.color = targetColor;
                yield return null;
            }
            targetAlpha = 0f;
            yield return new WaitForSeconds(secondToRemove);
        }
        TextRemove();
    }

    private void TextRemove()
    {
        _txt.text = null;
    }
}
