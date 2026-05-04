using System;
using Cysharp.Threading.Tasks;
using Photon.Deterministic;
using Photon.Realtime;
using Quantum;
using UnityEngine;

public class StartQuantumSimulationManager : MonoBehaviour
{
    [SerializeField] private RuntimeConfig runtimeConfig;
    [SerializeField] private RuntimePlayer runtimePlayer;

    public Action OnQuantumSessionStarted;

    private void Awake()
    {
        QuantumCallback.Subscribe(this, (CallbackGameStarted c) => OnGameStarted(c.Game, c.IsResync), game => game == QuantumRunner.Default.Game);

    }

    public async UniTask<bool> Matchmaking(string roomName, Action OnQuantumSessionStarted)
    {
        this.OnQuantumSessionStarted = OnQuantumSessionStarted;
        var connectionArguments = new MatchmakingArguments {
            PhotonSettings = PhotonServerSettings.Global.AppSettings,
            PluginName = "QuantumPlugin",
            RoomName = $"{roomName}",
            MaxPlayers = 2,
            CanOnlyJoin = false,
            UserId = Guid.NewGuid().ToString(),
        };
        
        RealtimeClient Client = default;
        
        try
        {
             Client = await MatchmakingExtensions.ConnectToRoomAsync(connectionArguments);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        
        StartQuantum(Client);
        return true;
    }

    private void StartQuantum(RealtimeClient client)
    {
        var sessionRunnerArguments = new SessionRunner.Arguments {
            RunnerFactory = QuantumRunnerUnityFactory.DefaultFactory,
            GameParameters = QuantumRunnerUnityFactory.CreateGameParameters,
            ClientId = client.UserId,
            RuntimeConfig = runtimeConfig,
            SessionConfig = QuantumDeterministicSessionConfigAsset.DefaultConfig,
            GameMode = DeterministicGameMode.Multiplayer,
            PlayerCount = 2,
            StartGameTimeoutInSeconds = 10,
            Communicator = new QuantumNetworkCommunicator(client),
        };
        
        SessionRunner.Start(sessionRunnerArguments);
    }
    
    private void OnGameStarted(QuantumGame callbackGame, bool callbackIsResync)
    {
        OnQuantumSessionStarted?.Invoke();
        
        
        runtimePlayer.PlayerNickname = PlayerDataManager.Instance.UserName;
        callbackGame.AddPlayer(runtimePlayer);
        
    }

}
