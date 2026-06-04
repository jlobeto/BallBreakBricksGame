using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResultUI : MonoBehaviour
{
    [SerializeField] private RectTransform parentObject;
    [SerializeField] private Image win;
    [SerializeField] private Image loss;
    [SerializeField] private TMP_Text finalScore;

    public void SetResult(bool won, int score)
    {
        win.enabled = won;
        loss.enabled = !won;
        finalScore.text = $"Final Score: {score.ToString()}";
    }

    public void Deactivate()
    {
        parentObject.gameObject.SetActive(false);
    }

    public void SetOnlineResult(bool won, int score)
    {
        SetResult(won, score);
        parentObject.anchorMin = new Vector2(0, 0f);
        parentObject.anchorMax = new Vector2(1, 1f);
        parentObject.anchoredPosition = new Vector2(0, 0);
    }
    
}
