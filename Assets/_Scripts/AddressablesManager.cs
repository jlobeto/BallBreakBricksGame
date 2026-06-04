using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class AddressablesManager : MonoBehaviour
{
    [SerializeField] private AssetReference loadingScreen;
    
    AsyncOperationHandle<SceneInstance> _loadingSceneHandle;
    
    void Start()
    {
        Initialize().Forget();
    }

    private async UniTask Initialize()
    {
        await Addressables.InitializeAsync(true);
    }
    
    public async UniTask<GameObject> ShowLoadingScreen()
    {
        var op = Addressables.InstantiateAsync(loadingScreen);
        await op;
        return op.Result;
    }

    public async UniTask<SceneInstance> LoadSceneAsync(string sceneName)
    {
        _loadingSceneHandle = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single, false);
        await _loadingSceneHandle;
        return _loadingSceneHandle.Result;
    }

    public void ReleaseSceneHandle()
    {
        if(_loadingSceneHandle.IsValid())
            Addressables.Release(_loadingSceneHandle);
    }

}
