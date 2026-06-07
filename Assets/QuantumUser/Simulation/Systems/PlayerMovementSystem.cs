using UnityEngine;
using UnityEngine.Assertions.Must;

namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerMovementSystem : SystemMainThreadFilter<PlayerMovementSystem.Filter>
    {
        public override bool StartEnabled => false;
        
        public struct Filter
        {
            public EntityRef Entity;
            public PlayerData* PlayerLink;
            public Transform2D* Transform;
        }
        
        public override void Update(Frame f, ref Filter filter)
        {
            var input = f.GetPlayerInput(filter.PlayerLink->playerRef);
            var mapConfig = f.FindAsset(f.RuntimeConfig.PlayerMapConfig);
            
            var playerRef = filter.PlayerLink->playerRef;
            var minXPos = mapConfig.minXPos + playerRef * mapConfig.mapWidth;
            var maxXPos = mapConfig.maxXPos + playerRef * mapConfig.mapWidth;
            
            if(input->HorizontalInput != 0)
            {
                FP horizontalInput = input->HorizontalInput;
                
                if (filter.Transform->Position.X <= minXPos)
                {
                    horizontalInput = FPMath.Clamp(horizontalInput, 0, 1);
                }
                else if(filter.Transform->Position.X >= maxXPos)
                {
                    horizontalInput = FPMath.Clamp(horizontalInput, -1, 0);
                }

                filter.Transform->Position += new FPVector2(filter.PlayerLink->speed * horizontalInput, 0);
                
                if (filter.Transform->Position.X < minXPos)
                {
                    filter.Transform->Position = new FPVector2(minXPos, 0);
                }
                else if (filter.Transform->Position.X > maxXPos)
                {
                    filter.Transform->Position = new FPVector2(maxXPos, 0);
                }
                
                if (filter.Transform->Position.X >= minXPos && filter.Transform->Position.X <= maxXPos)
                {
                    f.Signals.OnPlayerMoved(playerRef, filter.Transform->Position);
                }
            }
        }

    }
}
