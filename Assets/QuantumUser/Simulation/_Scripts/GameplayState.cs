using Photon.Deterministic;
using UnityEngine;

namespace Quantum
{
    public unsafe partial struct GameplayState
    {
        public void Initialize(Frame f)
        {
            scoresDict = f.AllocateDictionary<PlayerRef, int>();
            blocksLeft = f.AllocateDictionary<PlayerRef, int>();
        }

        public void SetBlockLeft(Frame f, PlayerRef playerRef, int blocksLeft)
        {
            var dict = f.ResolveDictionary(this.blocksLeft);
            if(!dict.ContainsKey(playerRef))
                dict.Add(playerRef, blocksLeft);
        }

        public void AddBlocks(Frame f, PlayerRef playerRef, int toAdd = 1)
        {
            AddOrRemoveBlock(f, playerRef, toAdd);
        }

        public void RemoveBlocks(Frame f, PlayerRef playerRef, int toRemove = 1)
        {
            var totalLeft = AddOrRemoveBlock(f, playerRef, -toRemove);
            if (totalLeft <= 0)
                OnEndOfMatch(f, playerRef);
        }

        public void CreatePlayerBall(Frame f, FPVector2 playerPosition, PlayerRef playerRef, EntityRef playerLinkEntityRef)
        {
            var commonBallConfig = f.FindAsset(f.RuntimeConfig.BallCommonConfig);
            var ballEntityRef = f.Create(commonBallConfig.BallPrototype);

            var ball = f.Unsafe.GetPointer<Ball>(ballEntityRef);
            ball->Initialize(commonBallConfig, playerRef, playerLinkEntityRef, ballEntityRef);
            
            var ballPhysics = f.Unsafe.GetPointer<PhysicsBody2D>(ballEntityRef);
            ballPhysics->Enabled = false;
            
            var ballTransform = f.Unsafe.GetPointer<Transform2D>(ballEntityRef);
            ballTransform->Position = playerPosition + FPVector2.Up;
        }

        public void AddScore(Frame f, PlayerRef playerRef, int score)
        {
            var dict = f.ResolveDictionary(scoresDict);
            if (!dict.ContainsKey(playerRef))
                dict.Add(playerRef, 0);
            
            dict[playerRef] += score;
            
            f.Events.OnScoreUpdate(playerRef, dict[playerRef]);
        }

        private void OnEndOfMatch(Frame f, PlayerRef winner)
        {
            f.SystemDisable<BallSystem>();
            f.SystemDisable<PlayerMovementSystem>();
            f.SystemDisable<ThrowBallSystem>();
            
            foreach (var _ in f.Unsafe.GetComponentBlockIterator<Ball>())
            {
                var ballPhysics = f.Unsafe.GetPointer<PhysicsBody2D>(_.Entity);
                ballPhysics->Enabled = false;
            }

            var losser = GameplayHelper.GetEnemy(f, winner);
            
            var scores = f.ResolveDictionary(scoresDict);
            scores.TryGetValue(winner, out int winnerScore);
            scores.TryGetValue(losser, out int losserScore);

            f.Events.OnMatchEnded(winner, winnerScore, losser, losserScore);
        }

        private int AddOrRemoveBlock(Frame f, PlayerRef playerRef, int blocks)
        {
            var dict = f.ResolveDictionary(this.blocksLeft);
            if (!dict.ContainsKey(playerRef)) return 0 ;
            
            dict[playerRef] += blocks;
            return dict[playerRef];
        }
    }
}
