using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class BlocksSystem : SystemSignalsOnly, ISignalOnBlockCollided
    {
        public void OnBlockCollided(Frame f, EntityRef blockRef, EntityRef ballRef)
        {
            if (!f.Unsafe.TryGetPointer<Block>(blockRef, out var block))
                return;

            var ball = f.Get<Ball>(ballRef);
            var dmg = ball.damage;

            block->lives -= dmg;
            if (block->lives <= 0)
            {
                f.Destroy(blockRef);
                f.Unsafe.GetPointerSingleton<GameplayState>()->RemoveBlocks(f, ball.owner);
            }
            else
                f.Events.OnBlockReceivedDamage(blockRef, block->lives);
            
        }
    }
}
