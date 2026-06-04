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
    [SerializeField] private PlayerResultUI[]  playerResults;
    
    [SerializeField] private Button continueButton;
    [SerializeField] private float showContinueButtonInSeconds = 2;

    private void Awake()
    {
        canvas.enabled = false;
        canvasGroup.alpha = 0;
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
        
        var winnerIsLeftPlayer = data.winner == 0;
        
        if (data.Game.Session.GameMode is DeterministicGameMode.Local)
        {
            var leftPlayerResult = winnerIsLeftPlayer ? playerResults[data.winner] : playerResults[data.losser];
            var rightPlayerResult = winnerIsLeftPlayer ? playerResults[data.losser] : playerResults[data.winner];
            
            leftPlayerResult.SetResult(winnerIsLeftPlayer, winnerIsLeftPlayer ? data.winnerScore : data.losserScore);
            rightPlayerResult.SetResult(!winnerIsLeftPlayer, !winnerIsLeftPlayer ? data.winnerScore : data.losserScore);
        }
        else
        {
            var playerResult = playerResults[0];
            
            var isLocalPlayerWinner = data.Game.PlayerIsLocal(data.winner);
            playerResult.SetOnlineResult(isLocalPlayerWinner, isLocalPlayerWinner ? data.winnerScore : data.losserScore);
            
            playerResults[1].Deactivate();
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