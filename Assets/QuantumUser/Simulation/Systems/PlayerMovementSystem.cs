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
                var data = f.Unsafe.GetPointerSingleton<PlayerMovementData>();
                FP horizontalInput = input->HorizontalInput;
                
                if (!data->canMoveLeft)
                    horizontalInput = FPMath.Clamp(horizontalInput, 0, 1);
                else if(!data->canMoveRight)
                    horizontalInput = FPMath.Clamp(horizontalInput, -1, 0);
                
                filter.Transform->Position += new FPVector2(filter.PlayerLink->speed * horizontalInput, 0);
            }
        }

    }
}
