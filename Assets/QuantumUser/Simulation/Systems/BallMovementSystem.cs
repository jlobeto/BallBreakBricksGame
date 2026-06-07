using Photon.Deterministic;
using UnityEngine;

namespace Quantum 
{ 
    public unsafe class BallMovementSystem : SystemMainThreadFilter<BallMovementSystem.Filter> , ISignalOnTriggerEnter2D
    { 
        public struct Filter 
        { 
            public EntityRef Entity; 
            public Transform2D* Transform; 
            public Ball* Ball; 
        } 

        public override void Update(Frame f, ref Filter ballFilter)
        {
            var ballTransform = ballFilter.Transform;
            var ball = ballFilter.Ball;
            
            ballTransform->Position += ballFilter.Ball->velocityVector * f.DeltaTime;
            
            var playerMapConfig = f.FindAsset(f.RuntimeConfig.PlayerMapConfig); 
            var ballMapConfig = f.FindAsset(f.RuntimeConfig.BallMapConfig); 
            
            CheckBallHitWithWall(ballTransform, ball,  ballMapConfig, playerMapConfig);
            if (CheckBallHitWithDeadZone(f, ballTransform, ball, ballMapConfig))
                return;
            
            var playerEntity = ballFilter.Ball->ownerEntityRef;
            PlayerData* player = default;
            if (!f.Unsafe.TryGetPointer(playerEntity, out Transform2D* playerTransform) || !f.Unsafe.TryGetPointer(playerEntity, out player))
                return;
            
            if (CheckBallPaddleCollision(ballTransform->Position, GetBallRadius(ball), playerTransform->Position, player->paddleSize)) 
            { 
                if (ballFilter.Ball->velocityVector.Y < 0)
                {
                    FP paddleHalfWidth = player->paddleSize / 2;
                        
                    FP hitFactor = (ballTransform->Position.X - playerTransform->Position.X) / paddleHalfWidth; 
                    hitFactor = FPMath.Clamp(hitFactor, -FP._1, FP._1);

                    FPVector2 newDirection = new FPVector2(hitFactor, FP._1);

                    ballFilter.Ball->velocityVector = newDirection.Normalized * ballFilter.Ball->speed;
                }
            }
        }

        private bool CheckBallHitWithDeadZone(Frame f, Transform2D* ballTransform, Ball* ball, BallMapConfig ballMapConfig)
        {
            if (ballTransform->Position.Y < ballMapConfig.deadZoneYPos) 
            {
                f.Signals.OnBallCollidedDeadZone(ball); 
                return true; 
            }

            return false;
        }

        private void CheckBallHitWithWall(Transform2D* ballTransform, Ball* ball, BallMapConfig ballMapConfig, PlayerMapConfig playerMapConfig)
        {
            var ownerPlayerRef = ball->owner;
            
            FPVector2 currentVelocityVector = ball->velocityVector;
            
            var minXPos = ballMapConfig.minXPos + ownerPlayerRef * playerMapConfig.mapWidth; 
            var maxXPos = ballMapConfig.maxXPos + ownerPlayerRef * playerMapConfig.mapWidth; 
            FP screenLimitY = ballMapConfig.maxYPos;
            
            bool wallHit = false;

            if (ballTransform->Position.X > maxXPos && currentVelocityVector.X > 0) 
            { 
                currentVelocityVector.X *= -1; 
                wallHit = true;
            } 
            else if (ballTransform->Position.X < minXPos && currentVelocityVector.X < 0) 
            { 
                currentVelocityVector.X *= -1; 
                wallHit = true;
            } 

            if (ballTransform->Position.Y > screenLimitY && currentVelocityVector.Y > 0) 
            { 
                currentVelocityVector.Y *= -1; 
                wallHit = true;
            } 

            if (wallHit)
                ball->velocityVector = currentVelocityVector.Normalized * ball->speed;
        }


        private bool CheckBallPaddleCollision(FPVector2 ballPos, FP ballRadius, FPVector2 paddlePos, FP paddleSize) 
        { 
            FP paddleHalfWidth = paddleSize/2;
            FP paddleHalfHeight = FP._0_50;

            return CheckCircleAabbCollision(ballPos, ballRadius, paddlePos, paddleHalfWidth, paddleHalfHeight);
        }

        private bool CheckCircleAabbCollision(FPVector2 circlePos, FP circleRadius, FPVector2 boxPos, FP boxHalfWidth, FP boxHalfHeight)
        {
            circleRadius = circleRadius > 0 ? circleRadius : FP._0_50;

            FP closestX = FPMath.Clamp(circlePos.X, boxPos.X - boxHalfWidth, boxPos.X + boxHalfWidth);
            FP closestY = FPMath.Clamp(circlePos.Y, boxPos.Y - boxHalfHeight, boxPos.Y + boxHalfHeight);

            FP distanceX = circlePos.X - closestX;
            FP distanceY = circlePos.Y - closestY;

            FP distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
            return distanceSquared <= (circleRadius * circleRadius);
        }

        private void ReflectBallFromAabb(Transform2D* ballTransform, Ball* ball, FPVector2 boxPos, FP boxHalfWidth, FP boxHalfHeight, FP ballRadius)
        {
            FPVector2 velocity = ball->velocityVector;
            FP distanceX = ballTransform->Position.X - boxPos.X;
            FP distanceY = ballTransform->Position.Y - boxPos.Y;
            FP overlapX = boxHalfWidth + ballRadius - FPMath.Abs(distanceX);
            FP overlapY = boxHalfHeight + ballRadius - FPMath.Abs(distanceY);

            if (overlapX < overlapY)
            {
                velocity.X *= -1;
                FP directionX = distanceX >= 0 ? FP._1 : -FP._1;
                ballTransform->Position.X += directionX * overlapX;
            }
            else
            {
                velocity.Y *= -1;
                FP directionY = distanceY >= 0 ? FP._1 : -FP._1;
                ballTransform->Position.Y += directionY * overlapY;
            }

            ball->velocityVector = velocity.Normalized * ball->speed;
        }

        private FP GetBallRadius(Ball* ball)
        {
            return ball->radius > 0 ? ball->radius : FP._0_50;
        }


        public void OnTriggerEnter2D(Frame f, TriggerInfo2D info)
        {
            if (f.Has<Block>(info.Other))
            {
                var ballTransform = f.Unsafe.GetPointer<Transform2D>(info.Entity);
                var ball = f.Unsafe.GetPointer<Ball>(info.Entity);
                var blockTransform = f.Get<Transform2D>(info.Other);

                ReflectBallFromAabb(ballTransform, ball, blockTransform.Position, FP._1 + FP._0_50, FP._0_50, GetBallRadius(ball));
                f.Signals.OnBlockCollided(info.Other, info.Entity);
            }
            
        }
    } 
}
