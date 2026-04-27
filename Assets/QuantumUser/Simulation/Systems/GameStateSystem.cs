using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class GameStateSystem : SystemMainThread, ISignalOnBallCollidedDeadZone
    {
        public override void OnInit(Frame f)
        {
            var state = f.Unsafe.GetOrAddSingletonPointer<GameplayState>();
            state->Initialize(f);
        }

        public override void Update(Frame frame)
        {
        }

        public void OnBallCollidedDeadZone(Frame f, Ball* ball)
        {
            var playerRef = ball->owner;
            CreateNewBallForPlayer(f, playerRef);
            f.Destroy(ball->entityRef);
        } 

        public PlayerLink GetPlayerLink(Frame f, PlayerRef playerRef)
        {
            foreach (var link in f.Unsafe.GetComponentBlockIterator<PlayerLink>())
            {
                if (playerRef == link.Component->playerRef)
                    return *link.Component;
            }

            return default;
        }

        private void CreateNewBallForPlayer(Frame f, PlayerRef playerRef)
        {
            var state = f.Unsafe.GetOrAddSingletonPointer<GameplayState>();
            
            var playerLink = GetPlayerLink(f, playerRef);

            var playerTransfrom = f.Unsafe.GetPointer<Transform2D>(playerLink.entityRef);
            
            state->CreatePlayerBall(f, playerTransfrom->Position, playerRef);
        }
    }
}
