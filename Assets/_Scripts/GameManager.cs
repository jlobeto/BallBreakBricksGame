using System;
using Cysharp.Threading.Tasks;
using Quantum;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    private static GameManager _instance;

    [SerializeField] AddressablesManager _addressablesManager;
    
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
    }

    public async UniTask GameModeSelected(bool isOnline)
    {
        await _addressablesManager.ShowLoadingScreen();
        await UniTask.WaitForSeconds(1);//hardcoded 1 second for UX purposes.
        await _addressablesManager.LoadSceneAsync("Level1");
    }
}
