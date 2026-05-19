using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class ScoreSystem : SystemSignalsOnly, ISignalOnBlockCollided, ISignalOnBallCollidedDeadZone
    {

        public void OnBlockCollided(Frame f, EntityRef blockRef, EntityRef ballRef)
        {
            var state = f.Unsafe.GetOrAddSingletonPointer<GameplayState>();
            var ownerPlayer = f.Get<Ball>(ballRef).owner;
            
            var scoreSystemConfig = f.FindAsset(f.RuntimeConfig.ScoreConfig);
            state->AddScore(f, ownerPlayer, scoreSystemConfig.blockHitScore);
            
        }

        public void OnBallCollidedDeadZone(Frame f, Ball* ball)
        {
            var state = f.Unsafe.GetOrAddSingletonPointer<GameplayState>();
            var ownerPlayer = ball->owner;
            var enemy = GameplayHelper.GetEnemy(f, ownerPlayer);
            
            var scoreSystemConfig = f.FindAsset(f.RuntimeConfig.ScoreConfig);
            state->AddScore(f, enemy, scoreSystemConfig.ballHitDeadzoneScore);
        }
    }
}
