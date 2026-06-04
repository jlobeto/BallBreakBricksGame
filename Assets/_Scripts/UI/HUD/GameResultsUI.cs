using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Photon.Deterministic;
using Quantum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class GameResultsUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private PlayerResult[]  playerResults;
    [SerializeField] private GameObject resultsUI;
    
    [SerializeField] private Button continueButton;
    [SerializeField] private float showContinueButtonInSeconds = 2;

    private void Awake()
    {
        canvas.enabled = false;
        canvasGroup.alpha = 0;
        
        resultsUI.SetActive(false);
    }

    void Start()
    {
        QuantumEvent.Subscribe<EventOnMatchEnded>(this, OnMatchEnded);
        continueButton.gameObject.SetActive(false);
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(ContinueToMainMenu);
    }

    private void OnMatchEnded(EventOnMatchEnded data)
    {
        canvas.enabled = true;
        canvasGroup.DOFade(1, 0.5f);
        
        ShowContinueButton().Forget();
        
        if (playerResults.Length != 2)
        {
            return;
        }
        
        resultsUI.SetActive(true);
        var winnerIsLeftPlayer = data.winner == 0;
        
        if (data.Game.Session.GameMode is DeterministicGameMode.Local)
        {
            var leftPlayerResult = winnerIsLeftPlayer ? playerResults[data.winner] : playerResults[data.losser];
            var rightPlayerResult = winnerIsLeftPlayer ? playerResults[data.losser] : playerResults[data.winner];
            
            leftPlayerResult.win.enabled = winnerIsLeftPlayer;
            leftPlayerResult.loss.enabled = !winnerIsLeftPlayer;
            
            rightPlayerResult.win.enabled = !winnerIsLeftPlayer;
            rightPlayerResult.loss.enabled = winnerIsLeftPlayer;

            leftPlayerResult.finalScore.text = $"Final Score: {(winnerIsLeftPlayer ? data.winnerScore : data.losserScore)}";
            rightPlayerResult.finalScore.text = $"Final Score: {(!winnerIsLeftPlayer ? data.winnerScore : data.losserScore)}";
        }
        else
        {
            var playerResult = playerResults[0];
            playerResult.parent.anchorMax = new Vector2(1, 0.5f);
            playerResult.parent.anchoredPosition = new Vector2(0, 0);
            playerResult.finalScore.text = $"Final Score: {(winnerIsLeftPlayer ? data.winnerScore : data.losserScore)}";
            
            playerResults[1].parent.gameObject.SetActive(false);
        }
    }

    private async UniTask ShowContinueButton()
    {
        await UniTask.WaitForSeconds(showContinueButtonInSeconds);
        continueButton.gameObject.SetActive(true);
    }
    
    private void ContinueToMainMenu()
    {
        continueButton.onClick.RemoveAllListeners();
        QuantumEvent.UnsubscribeListener(this);

        EventBus.Publish(new EventOnShutdownQuantum());
    }
}

[Serializable]
public struct PlayerResult
{
    public RectTransform parent;
    public Image win;
    public Image loss;
    public TMP_Text finalScore;
}
