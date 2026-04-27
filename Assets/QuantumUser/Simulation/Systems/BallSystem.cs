using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class BallSystem : SystemMainThreadFilter<BallSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Ball*  Ball;
            public PhysicsBody2D* PhysicsBody;
        }
        

        public override void Update(Frame frame, ref Filter filter)
        {
            
            
        }

        
    }
}
