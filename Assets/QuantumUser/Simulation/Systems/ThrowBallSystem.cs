namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class ThrowBallSystem : SystemMainThreadFilter<ThrowBallSystem.Filter>
    {
        public override bool StartEnabled => false;

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
                
                    InitializeBallPhysics(_.Component);
                    break;
                }
            }
        }
        
        private void InitializeBallPhysics(Ball* ball)
        {
            if (ball->wasThrown) return;

            ball->wasThrown = true;
            ball->velocityVector= new FPVector2(ball->speed);
        }
    }
}
