
using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class CollisionMainSystem : SystemSignalsOnly, ISignalOnTriggerEnter2D, ISignalOnCollisionEnter2D
    {
        public void OnTriggerEnter2D(Frame f, TriggerInfo2D info)
        {
            if (f.TryGet<Wall>(info.Other, out var wall))
            {
                Debug.Log($"Wall! - > {wall.isLeft}");
                var movementData = f.Unsafe.GetOrAddSingletonPointer<PlayerMovementData>();
                movementData->canMoveLeft = !wall.isLeft;
                movementData->canMoveRight = wall.isLeft;
            }
        }

        public void OnCollisionEnter2D(Frame f, CollisionInfo2D info)
        {
            Debug.LogError("collision!!!!");
        }
    }
}
