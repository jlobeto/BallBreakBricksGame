using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class BallMovementSystem : SystemMainThreadFilter<BallMovementSystem.Filter>
    {
        public override bool StartEnabled => false;

        public override void Update(Frame frame, ref Filter filter)
        {

            //filter.PhysicsBody->Velocity = new FPVector2(FP._10);

        }

        public struct Filter
        {
            public EntityRef Entity;
            public Ball*  Ball;
            public PhysicsBody2D* PhysicsBody;
        }
    }
}
