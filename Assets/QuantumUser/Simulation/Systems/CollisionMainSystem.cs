
using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class CollisionMainSystem : SystemSignalsOnly, ISignalOnCollisionExit2D, ISignalOnTriggerEnter2D
    {
        public override bool StartEnabled => false;

        public void OnCollisionExit2D(Frame f, ExitInfo2D info)
        {
            if (f.Has<Block>(info.Other))
            {
                Debug.Log($"Collision Enter - {info.Other.GetName(f)}");
                //f.Signals.OnBlockCollided(info.Other, info.Entity);
            }

            if (f.Has<PlayerData>(info.Other))
            {
                var ballPhysics = f.Unsafe.GetPointer<PhysicsBody2D>(info.Entity);
                Debug.Log(ballPhysics->Velocity);
                //ballPhysics->Velocity = new FPVector2(20, ballPhysics->Velocity.Y);
            }
        }
        
        /*float HitFactor(FPVector2 ballPos, FPVector2 racketPos, FP racketWidth)
        {
            // ASCII art illustration of hitFactor:
            // Hit on left = -1, Hit on center = 0, Hit on right = 1
            return (ballPos.x - racketPos.x) / racketWidth;
        }*/

        public void OnTrigger2D(Frame f, TriggerInfo2D info)
        {

        }

        public void OnTriggerEnter2D(Frame f, TriggerInfo2D info)
        {
            if(f.Has<DeadZone>(info.Other))
            {
                f.Signals.OnBallCollidedDeadZone(f.Unsafe.GetPointer<Ball>(info.Entity));
            }


        }

        public void OnTriggerExit2D(Frame f, TriggerInfo2D info)
        {
            
        }
    }
}
