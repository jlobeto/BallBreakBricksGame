using UnityEngine;

namespace Quantum
{
    public partial struct Ball
    {
        public void Initialize(BallConfig config, PlayerRef playerRef, EntityRef playerEntityRef,
            EntityRef ballEntityRef)
        {
            damage = config.damage;
            entityRef = ballEntityRef;
            speed = config.InitialSpeed;
            owner = playerRef;
            ownerEntityRef = playerEntityRef;
        }
    }
}
