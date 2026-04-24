using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class BlocksSystem : SystemSignalsOnly, ISignalOnBlockCollided
    {
        public void OnBlockCollided(Frame f, PlayerRef playerRef, EntityRef blockRef, EntityRef ballRef)
        {
            if (!f.Unsafe.TryGetPointer<Block>(blockRef, out var block))
                return;

            var dmg = f.Get<Ball>(ballRef).damage;

            block->lives -= dmg;
            if (block->lives <= 0)
            {
                f.Destroy(blockRef);
            }
            else
            {
                f.Events.OnBlockReceivedDamage(blockRef, block->lives);
            }
        }
    }
}
