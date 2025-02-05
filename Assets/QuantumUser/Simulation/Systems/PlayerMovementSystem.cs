namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerMovementSystem : SystemMainThreadFilter<PlayerMovementSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            //filter.Transform->Position += new FPVector2(FP._1, 0);
        }

        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* PlayerLink;
            public Transform2D* Transform;
        }
    }
}
