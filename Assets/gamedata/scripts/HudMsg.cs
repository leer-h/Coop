using UnityEngine;
using System.Collections;

public class HudMsg : MonoBehaviour
{
    [SerializeField] private TextMesh _txt;
    public void SetHudMsg(string str, Vector3 pos, float size, bool useAnm, float secondToRemove)
    {
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

        _txt.transform.position = pos;
        _txt.text = str;
        _txt.characterSize = size;
    }

    IEnumerator LerpAnm(float secondToRemove)
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
