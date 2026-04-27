using Photon.Deterministic;
using UnityEngine;

namespace Quantum
{
    public unsafe partial struct GameplayState
    {
        public void Initialize(Frame f)
        {
            scoresDict = f.AllocateDictionary<PlayerRef, int>();
        }
        
        public void CreatePlayerBall(Frame f, FPVector2 playerPosition, PlayerRef playerRef)
        {
            var commonBallConfig = f.FindAsset(f.RuntimeConfig.BallCommonConfig);
            var ballEntityRef = f.Create(commonBallConfig.BallPrototype);

            var ball = f.Unsafe.GetPointer<Ball>(ballEntityRef);
            ball->Initialize(commonBallConfig, playerRef, ballEntityRef);
            
            var ballPhysics = f.Unsafe.GetPointer<PhysicsBody2D>(ballEntityRef);
            ballPhysics->Enabled = false;
            
            var ballTransform = f.Unsafe.GetPointer<Transform2D>(ballEntityRef);
            ballTransform->Position = playerPosition + FPVector2.Up;
        }
    }
}
