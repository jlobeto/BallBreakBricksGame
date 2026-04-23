
using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class CollisionMainSystem : SystemSignalsOnly, ISignalOnTriggerEnter2D, ISignalOnTriggerExit2D
    {
        public override bool StartEnabled => false;

        public void OnTriggerEnter2D(Frame f, TriggerInfo2D info)
        {
            
        }

        public void OnTriggerExit2D(Frame f, ExitInfo2D info)
        {
        }

    }
}
