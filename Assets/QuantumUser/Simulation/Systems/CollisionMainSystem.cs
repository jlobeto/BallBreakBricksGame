
using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class CollisionMainSystem : SystemSignalsOnly, ISignalOnTriggerEnter2D
    {
        public void OnTriggerEnter2D(Frame f, TriggerInfo2D info)
        {
            
        }

    }
}
