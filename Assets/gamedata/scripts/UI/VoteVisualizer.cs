using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class VoteVisualizer : MonoBehaviour
{
    [SerializeField] private Image[] voteBars;
    [SerializeField] private float maxBarHeight = 100f;
    [SerializeField] private float minBarHeight = 50f;
    [SerializeField] private float animationDuration = 0.3f;

    private Dictionary<int, int> votes = new Dictionary<int, int>();

    void Awake()
    {
        for (int i = 0; i < voteBars.Length; i++)
        {
            int key = i + 1;
            votes[key] = 0;

            RectTransform rt = voteBars[i].rectTransform;
            rt.pivot = new Vector2(0.5f, 0f);
        }
    }

    public void UpdateVoteCounts(Dictionary<int, int> voteCounts)
    {
        foreach (var kvp in voteCounts)
        {
            if (votes.ContainsKey(kvp.Key))
                votes[kvp.Key] = kvp.Value;
        }

        UpdateBars();
    }

    private void UpdateBars()
    {
        int maxVotes = 1;
        foreach (var count in votes.Values)
            if (count > maxVotes) maxVotes = count;

        for (int i = 0; i < voteBars.Length; i++)
        {
            int key = i + 1;
            float normalized = (float)votes[key] / maxVotes;

            float targetHeight = Mathf.Max(minBarHeight, normalized * maxBarHeight);

            RectTransform rt = voteBars[i].rectTransform;
            Vector2 size = rt.sizeDelta;

            rt.DOSizeDelta(new Vector2(size.x, targetHeight), animationDuration);
        }
    }
}
