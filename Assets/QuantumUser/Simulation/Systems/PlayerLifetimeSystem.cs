using UnityEngine;
using UnityEngine.Assertions.Must;

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
            playerLink->speed = 1;

            var transform2D = f.Unsafe.GetPointer<Transform2D>(entity);
            transform2D->Position = new FPVector2(-44, 0);
            
            var data = f.Unsafe.GetOrAddSingletonPointer<PlayerMovementData>();
            data->canMoveRight = data->canMoveLeft = true;
            
            Debug.Log($"OnPlayerAdded - {player}");
        }
    }
}
