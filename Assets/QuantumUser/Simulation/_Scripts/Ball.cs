using UnityEngine;

namespace Quantum
{
    public partial struct Ball
    {
        public void Initialize(BallConfig config, PlayerRef playerRef, EntityRef ballEntityRef)
        {
            damage = config.damage;
            entityRef = ballEntityRef;
            initialSpeed = config.InitialSpeed;
            owner = playerRef;
        }
    }
}
