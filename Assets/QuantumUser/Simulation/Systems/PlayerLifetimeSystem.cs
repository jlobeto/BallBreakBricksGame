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

            var p = f.Unsafe.GetPointer<PlayerLink>(entity);
            p->playerRef = player;
            p->speed = 5;
            
            Debug.Log($"OnPlayerAdded - {player}");
            
            
        }
    }
}
