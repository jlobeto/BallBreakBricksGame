using Cysharp.Threading.Tasks;
using DG.Tweening;
using Photon.Deterministic;
using Quantum;
using UnityEngine;
using UnityEngine.Serialization;

public class UserControlsUI : QuantumMonoBehaviour, ICommand
{
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private CanvasGroup leftPlayerCanvas;
    [SerializeField] private CanvasGroup rightPlayerCanvas;
    [SerializeField] private CanvasGroup onlinePlayerCanvas;
    [SerializeField] private float fadeDuration = 1;
    [SerializeField] private float showTime = 2;

    private bool _isLocalGameplay;
    
    void Awake()
    {
        QuantumEvent.Subscribe<EventOnPlayerAdded>(this, OnPlayerAdded);
        myCanvas.enabled = false;
    }
    
    private void OnPlayerAdded(EventOnPlayerAdded data)
    {
        QuantumEvent.UnsubscribeListener<EventOnPlayerAdded>(this);
        myCanvas.enabled = true;
        leftPlayerCanvas.alpha = 0;
        rightPlayerCanvas.alpha = 0;
        onlinePlayerCanvas.alpha = 0;
        
        _isLocalGameplay = data.Game.Session.GameMode is DeterministicGameMode.Local;
    }

    public async UniTask Execute()
    {
        if (_isLocalGameplay)
            await HandleLocalGameplay();
        else
            await HandleOnlineGameplay();
    }

    private async UniTask HandleOnlineGameplay()
    {
        await onlinePlayerCanvas.DOFade(1, fadeDuration).AsyncWaitForCompletion();
        await UniTask.WaitForSeconds(showTime);
        await onlinePlayerCanvas.DOFade(0, fadeDuration).AsyncWaitForCompletion();
        myCanvas.enabled = false;
    }

    private async UniTask HandleLocalGameplay()
    {
        leftPlayerCanvas.DOFade(1, fadeDuration);
        await rightPlayerCanvas.DOFade(1, fadeDuration).AsyncWaitForCompletion();
        await  UniTask.WaitForSeconds(showTime);
        
        leftPlayerCanvas.DOFade(0, fadeDuration);
        await rightPlayerCanvas.DOFade(0, fadeDuration).AsyncWaitForCompletion();
        
        myCanvas.enabled = false;
    }
}
