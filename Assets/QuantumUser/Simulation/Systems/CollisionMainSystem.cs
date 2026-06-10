
using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class CollisionMainSystem : SystemSignalsOnly, ISignalOnCollisionExit2D, ISignalOnTriggerExit2D
    {

        public void OnCollisionExit2D(Frame f, ExitInfo2D info)
        {
            if (f.Has<Block>(info.Other))
            {
                //Debug.Log($"Collision Enter - {info.Other.GetName(f)}");
                f.Signals.OnBlockCollided(info.Other, info.Entity);
            }
        }
        

        public void OnTriggerExit2D(Frame f, ExitInfo2D info)
        {
            if(f.Has<DeadZone>(info.Other))
            {
                f.Signals.OnBallCollidedDeadZone(f.Unsafe.GetPointer<Ball>(info.Entity));
            }
            
        }
    }
}
