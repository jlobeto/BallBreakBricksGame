using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class AddressablesManager : MonoBehaviour
{
    [SerializeField] private AssetReference loadingScreen;
    
    void Start()
    {
        Initialize().Forget();
    }

    private async UniTask Initialize()
    {
        await Addressables.InitializeAsync(true);
    }
    
    public async UniTask ShowLoadingScreen()
    {
        var op = Addressables.InstantiateAsync(loadingScreen);
        await op;
    }

    public async UniTask<SceneInstance> LoadSceneAsync(string sceneName)
    {
        var op = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single, false);
        await op;
        return op.Result;
    }

}
