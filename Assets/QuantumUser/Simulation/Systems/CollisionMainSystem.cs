
using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class CollisionMainSystem : SystemSignalsOnly, ISignalOnTriggerEnter2D, ISignalOnTriggerExit2D
    {
        public void OnTriggerEnter2D(Frame f, TriggerInfo2D info)
        {
            if (f.TryGet<Wall>(info.Other, out var wall))
            {
                Debug.Log($"Wall! - > Frame {f.Number}");
                var movementData = f.Unsafe.GetOrAddSingletonPointer<PlayerMovementData>();
                movementData->canMoveLeft = !wall.isLeft;
                movementData->canMoveRight = wall.isLeft;
                
            }
        }

        public void OnTriggerExit2D(Frame f, ExitInfo2D info)
        {
            if (f.TryGet<Wall>(info.Other, out var wall))
            {
                var movementData = f.Unsafe.GetOrAddSingletonPointer<PlayerMovementData>();
                movementData->canMoveLeft = true;
                movementData->canMoveRight = true;
            }
        }
    }
}
