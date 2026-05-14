using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Photon.Deterministic;
using Photon.Realtime;
using Quantum;
using Quantum.Menu;
using UnityEngine;

public class QuantumService : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks
{
    [SerializeField] QuantumMenuConnectionBehaviourSDK quantumConnectionSdk;

    [SerializeField] private RuntimeConfig runtimeConfig;
    [SerializeField] private RuntimePlayer runtimePlayer;

    private RealtimeClient _client;
    private bool _isPingingRegions;
    private string _bestRegion;
    private string _roonName;
    private bool _callClientService;

    private void Awake()
    {
        //QuantumCallback.Subscribe(this, (CallbackGameStarted c) => OnGameStarted(c.Game, c.IsResync), game => game == QuantumRunner.Default.Game);
    }

    private void Start()
    {
        _client = new RealtimeClient();
        _client.AddCallbackTarget(this);
        _client.StateChanged += OnClientStateChanged;
        StartCoroutine(CallClientService());
    }

    private IEnumerator CallClientService()
    {
        while (_callClientService)
        {
            yield return new WaitForSeconds(0.1f);//recommended by photon (10 times per second).
            _client?.Service();
        }
    }

    private void OnDestroy()
    {
        if (_client == null)
            return;

        _client.StateChanged -= OnClientStateChanged;
        _client.RemoveCallbackTarget(this);
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        var result = regionHandler.PingMinimumOfRegions(PingDone, PlayerDataManager.Instance.PhotonRegionSummary);
        if (!result)
            _isPingingRegions = false;
    }

    private void PingDone(RegionHandler regionHandler)
    {
        Debug.Log($"Ping Done: BestRegion:{regionHandler.BestRegion.Code}-Ping:{regionHandler.BestRegion.Ping}");
        PlayerDataManager.Instance.SavePhotonRegionSummary(regionHandler.SummaryToCache);
        _isPingingRegions = false;
        _bestRegion = regionHandler.BestRegion.Code;
        
        EventBus.Publish(new EventOnRegionReceived()
        {
            ping =  regionHandler.BestRegion.Ping,
            regionCode =  regionHandler.BestRegion.Code,
        });
    }

    
    public async UniTask<bool> Matchmaking(string roomName)
    {
        if (_isPingingRegions)
        {
            Debug.Log($"[{GetType()}]: Matchmaking() -> Still Pinging for Regions...");
            EventBus.Publish(new EventOnLoadingChanged(){loadingTitle = "Pinging Regions..."});
            await UniTask.WaitUntil(() => !_isPingingRegions);
        }

        _roonName = roomName;
        
        try
        {
            // IMPORTANT: this API disconnects the client used for region discovery.
            // Use a temporary client so it cannot disconnect our gameplay client.
            var regionClient = new RealtimeClient();
            var regionHandler = await regionClient.ConnectToNameserverAndWaitForRegionsAsync(PhotonServerSettings.Global.AppSettings);
            if (regionHandler?.BestRegion != null)
            {
                _bestRegion = regionHandler.BestRegion.Code;
                PlayerDataManager.Instance.SavePhotonRegionSummary(regionHandler.SummaryToCache);
                EventBus.Publish(new EventOnRegionReceived()
                {
                    ping = regionHandler.BestRegion.Ping,
                    regionCode = regionHandler.BestRegion.Code,
                });
            }

            var appSettings = new AppSettings(PhotonServerSettings.Global.AppSettings)
            {
                FixedRegion = _bestRegion
            };

            await _client.ConnectUsingSettingsAsync(appSettings);
            
            var connectionArguments = new MatchmakingArguments {
                PhotonSettings =  appSettings,
                PluginName = "QuantumPlugin",
                RoomName = $"{_roonName}",
                MaxPlayers = 2,
                CanOnlyJoin = false,
                UserId = Guid.NewGuid().ToString(),
                NetworkClient = _client
            };
            
            await _client.JoinOrCreateRoomAsync(BuildEnterRoomArgs(connectionArguments));
            
            EventBus.Publish(new EventOnLoadingChanged(){loadingTitle = "Starting Game..."});
            var sessionRunnerArguments = new SessionRunner.Arguments {
                RunnerFactory = QuantumRunnerUnityFactory.DefaultFactory,
                GameParameters = QuantumRunnerUnityFactory.CreateGameParameters,
                ClientId = _client.UserId,
                RuntimeConfig = runtimeConfig,
                SessionConfig = QuantumDeterministicSessionConfigAsset.DefaultConfig,
                GameMode = DeterministicGameMode.Multiplayer,
                PlayerCount = 2,
                StartGameTimeoutInSeconds = 10,
                Communicator = new QuantumNetworkCommunicator(_client),
            };

            await SessionRunner.StartAsync(sessionRunnerArguments);
            
            EventBus.Publish(new EventOnLoadingChanged(){loadingTitle = "Game Loaded..."}); 
        
            runtimePlayer.PlayerNickname = PlayerDataManager.Instance.UserName;
            QuantumRunner.DefaultGame.AddPlayer(runtimePlayer);
        
            EventBus.Publish(new EventOnQuantumGameStarted());
            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }
        
    }
    
    private EnterRoomArgs BuildEnterRoomArgs(MatchmakingArguments arguments)
    {
        return new EnterRoomArgs
        {
            RoomName = arguments.RoomName,
            Lobby = arguments.Lobby,
            Ticket = arguments.Ticket,
            ExpectedUsers = arguments.ExpectedUsers,
            RoomOptions = arguments.CustomRoomOptions ?? new RoomOptions()
            {
                MaxPlayers = (byte)arguments.MaxPlayers,
                IsOpen = arguments.IsRoomOpen.HasValue ? arguments.IsRoomOpen.Value : true,
                IsVisible = arguments.IsRoomVisible.HasValue ? arguments.IsRoomVisible.Value : true,
                DeleteNullProperties = true,
                PlayerTtl = arguments.PlayerTtlInSeconds * 1000,
                EmptyRoomTtl = arguments.EmptyRoomTtlInSeconds * 1000,
                Plugins = arguments.Plugins,
                SuppressRoomEvents = false,
                SuppressPlayerInfo = false,
                PublishUserId = true,
                CustomRoomProperties = arguments.CustomProperties,
                CustomRoomPropertiesForLobby = arguments.CustomLobbyProperties,
            }
        };
    }


    #region IConnectionCallbacks

    public void OnConnected()
    {
        Debug.LogError($"OnConnected()");
    }

    public void OnConnectedToMaster()
    {
        Debug.LogError($"OnConnectedToMaster()");
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError($"OnDisconnected() {cause}");
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.LogError($"OnCustomAuthenticationResponse()");    

    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.LogError($"OnCustomAuthenticationFailed()");    

    }

    #endregion

    #region IMatchmakingCallbacks

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
    }

    public void OnCreatedRoom()
    {
        Debug.LogError($"OnCreatedRoom()");    
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"OnCreateRoomFailed()");    

    }

    public void OnJoinedRoom()
    {
        Debug.LogError($"OnJoinedRoom()");
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"OnJoinRoomFailed()");    

    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError($"OnJoinRandomFailed()");    

    }

    public void OnLeftRoom()
    {
        Debug.LogError($"OnLeftRoom()");    

    }

    #endregion
    
    

    private void OnClientStateChanged(ClientState arg1, ClientState arg2)
    {
        Debug.Log($"OnClientStateChanged() From {arg1} >> {arg2}");
    }

}
