using System;
using Cysharp.Threading.Tasks;
using Quantum;
using Quantum.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    private static GameManager _instance;

    [SerializeField] AddressablesManager _addressablesManager;
    [SerializeField] QuantumService quantumService;
    
    
    private PlayerDataManager _playerDataManager;
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    
        EventBus.AddListener<EventOnPlayClicked>(OnPlayModeClicked);
    }

    public async UniTask GoBackToMainMenu()
    {
        var loadingScreen = await _addressablesManager.ShowLoadingScreen();
        quantumService.ShutdownQuantum();
        
        await SceneManager.LoadSceneAsync(0);
        _addressablesManager.ReleaseSceneHandle();

        Resources.UnloadUnusedAssets();
        GC.Collect();
        
        Destroy(loadingScreen);
    }

    private void OnPlayModeClicked(EventOnPlayClicked data)
    {
        if (data.IsOnlineMatch)
            OnlinePath(data.RoomId).Forget();
        else
            OfflinePath().Forget();
    }

    private async UniTask OfflinePath()
    {
        await _addressablesManager.ShowLoadingScreen();
        var scene = await _addressablesManager.LoadSceneAsync("Level1");
        await UniTask.WaitForEndOfFrame();
        await UniTask.WaitForEndOfFrame();
        await scene.ActivateAsync();
        quantumService.StartLocalSimulation().Forget();

    }

    private async UniTask OnlinePath(string roomName)
    {
        await _addressablesManager.ShowLoadingScreen();
        var scene = await _addressablesManager.LoadSceneAsync("Level1");
        await scene.ActivateAsync();
        quantumService.Matchmaking(roomName).Forget();

    }
}
