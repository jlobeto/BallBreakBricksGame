namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class ThrowBallSystem : SystemMainThreadFilter<ThrowBallSystem.Filter>
    {

        public struct Filter
        {
            public EntityRef Entity;
            public PlayerData* Player;
        }
        
        public override void Update(Frame f, ref Filter filter)
        {
            var input = f.GetPlayerInput(filter.Player->playerRef);
            if (input->ThrowBall.WasPressed)
            {
                foreach (var _ in f.Unsafe.GetComponentBlockIterator<Ball>())
                {
                    if(_.Component->owner !=  filter.Player->playerRef) continue;
                    if(_.Component->wasThrown) continue;
                
                    InitializeBallPhysics(_.Component, f.Unsafe.GetPointer<PhysicsBody2D>(_.Entity));
                    break;
                }
            }
        }
        
        private void InitializeBallPhysics(Ball* ball, PhysicsBody2D* ballPhysics)
        {
            if (ball->wasThrown) return;

            ball->wasThrown = true;
            ballPhysics->Enabled = true;
            ballPhysics->Velocity = new FPVector2(ball->speed);
        }
    }
}
