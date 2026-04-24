using UnityEngine;
using UnityEngine.Assertions.Must;

namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerMovementSystem : SystemMainThreadFilter<PlayerMovementSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* PlayerLink;
            public Transform2D* Transform;
        }
        
        public override void Update(Frame f, ref Filter filter)
        {
            var input = f.GetPlayerInput(filter.PlayerLink->playerRef);
            
            if(input->HorizontalInput != 0)
            {
                FP horizontalInput = input->HorizontalInput;
                
                //set this constrains as quantum assets as a level config file 
                if (filter.Transform->Position.X <= -11)
                {
                    horizontalInput = FPMath.Clamp(horizontalInput, 0, 1);
                }
                else if(filter.Transform->Position.X >= 11)
                {
                    horizontalInput = FPMath.Clamp(horizontalInput, -1, 0);
                }

                filter.Transform->Position += new FPVector2(filter.PlayerLink->speed * horizontalInput, 0);
                if (filter.Transform->Position.X < -11)
                {
                    filter.Transform->Position = new FPVector2(-11, 0);
                }
                else if (filter.Transform->Position.X > 11)
                {
                    filter.Transform->Position = new FPVector2(11, 0);
                }
            }
        }

    }
}
