using Quantum.Prototypes;
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
            playerLink->speed = 2;

            var playerTransform = f.Unsafe.GetPointer<Transform2D>(entity);
            playerTransform->Position = new FPVector2(0, 0);
            
            Debug.Log($"OnPlayerAdded - {player}");
            
            CreatePlayerBall(f, playerTransform->Position);
        }

        private void CreatePlayerBall(Frame f, FPVector2 playerPosition)
        {
            var commonBallConfig = f.FindAsset(f.RuntimeConfig.BallCommonConfig);
            var ballEntityRef = f.Create(commonBallConfig.BallPrototype);

            var ball = f.Unsafe.GetPointer<Ball>(ballEntityRef);
            ball->Initialize(commonBallConfig);
            
            var ballPhysics = f.Unsafe.GetPointer<PhysicsBody2D>(ballEntityRef);
            ballPhysics->Velocity = new FPVector2(commonBallConfig.InitialSpeed);
            
            var ballTransform = f.Unsafe.GetPointer<Transform2D>(ballEntityRef);
            ballTransform->Position = playerPosition + FPVector2.Up * 2;
        }
    }
}
