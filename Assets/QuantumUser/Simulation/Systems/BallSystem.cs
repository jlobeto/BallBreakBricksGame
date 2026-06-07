using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class BallSystem : SystemMainThreadFilter<BallSystem.Filter>, ISignalOnPlayerMoved
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


        public void OnPlayerMoved(Frame f, PlayerRef playerRef, FPVector2 position)
        {
            foreach (var _ in f.Unsafe.GetComponentBlockIterator<Ball>())
            {
                if (_.Component->owner == playerRef)
                {
                    if (_.Component->wasThrown) break;
                    
                    var ballTransform = f.Unsafe.GetPointer<Transform2D>(_.Entity);
                    ballTransform->Position = position + FPVector2.Up;
                    break;
                }
            }
        }
    }
}
