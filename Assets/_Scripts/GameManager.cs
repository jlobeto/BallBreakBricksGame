using System;
using Cysharp.Threading.Tasks;
using Quantum;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    private static GameManager _instance;

    [SerializeField] AddressablesManager _addressablesManager;
    [SerializeField] StartQuantumSimulationManager startQuantumSimulationManager;
    [SerializeField] private MenuUI menuUI;
    
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
        
        menuUI.OnPlayModeClicked += OnPlayModeClicked;
    }

    private void OnPlayModeClicked(bool isOnlineMode, string roomName)
    {
        if (isOnlineMode)
            OnlinePath(roomName).Forget();
        else
            OfflinePath().Forget();
    }

    private async UniTask OfflinePath()
    {
        await ShowLoadingScreen();
        var scene = await _addressablesManager.LoadSceneAsync("Level1");
        await scene.ActivateAsync();

    }

    private async UniTask OnlinePath(string roomName)
    {
        await ShowLoadingScreen();
        var scene = await _addressablesManager.LoadSceneAsync("Level1");
        scene.ActivateAsync();
        await startQuantumSimulationManager.Matchmaking(roomName, null);
    }

    private async UniTask ShowLoadingScreen()
    {
        await _addressablesManager.ShowLoadingScreen();
        await UniTask.WaitForSeconds(1);//hardcoded 1 second for UX purposes.   
    }
}
