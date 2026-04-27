using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerLifetimeSystem : SystemSignalsOnly, ISignalOnPlayerConnected, ISignalOnPlayerAdded
    {
        public void OnPlayerConnected(Frame f, PlayerRef player)
        {
            Debug.Log($"OnPlayerConnected - {player}");
        }

        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            var runtimePlayer = f.GetPlayerData(player);
            var entity = f.Create(runtimePlayer.PlayerAvatar);

            var playerLink = f.Unsafe.GetPointer<PlayerLink>(entity);
            playerLink->playerRef = player;
            playerLink->entityRef = entity;
            playerLink->speed = 2;

            var playerTransform = f.Unsafe.GetPointer<Transform2D>(entity);
            playerTransform->Position = new FPVector2(0, 0);
            
            Debug.Log($"OnPlayerAdded - {player}");

            var gameState = f.Unsafe.GetOrAddSingletonPointer<GameplayState>();
            gameState->CreatePlayerBall(f, playerTransform->Position, player);
        }

        
    }
}
