using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

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

}
