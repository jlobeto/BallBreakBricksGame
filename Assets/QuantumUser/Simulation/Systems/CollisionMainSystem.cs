
using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class CollisionMainSystem : SystemSignalsOnly, ISignalOnCollisionEnter2D
    {
        
        public void OnCollisionEnter2D(Frame f, CollisionInfo2D info)
        {
            if (f.Has<Block>(info.Other))
            {
                //Debug.Log($"Collision Enter - {info.Other.GetName(f)}");
                f.Signals.OnBlockCollided(0, info.Other, info.Entity);
            }
        }
    }
}
