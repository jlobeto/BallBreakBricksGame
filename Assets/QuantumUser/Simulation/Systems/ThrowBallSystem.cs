namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class ThrowBallSystem : SystemMainThreadFilter<ThrowBallSystem.Filter>
    {
        
        public struct Filter
        {
            public EntityRef Entity;
            public Ball*  Ball;
            public PhysicsBody2D* PhysicsBody;
        }
        
        public override void Update(Frame frame, ref Filter filter)
        {
            for (int player = 0; player < frame.MaxPlayerCount; player++) 
            {
                var command = frame.GetPlayerCommand(player) as ThrowBallCommand;
                if(command != null)
                    InitializeBallPhysics(frame, filter.Ball, filter.PhysicsBody);
            }
        }
        
        private void InitializeBallPhysics(Frame f, Ball* ball, PhysicsBody2D* ballPhysics)
        {
            if (ball->wasThrown) return;

            ball->wasThrown = true;
            ballPhysics->Enabled = true;
            ballPhysics->Velocity = new FPVector2(ball->initialSpeed);
        }
    }
}
