using UnityEngine;

namespace Quantum
{
    public partial struct Ball
    {
        public void Initialize(BallConfig config, PlayerRef playerRef)
        {
            damage = config.damage;
            initialSpeed = config.InitialSpeed;
            owner = playerRef;
        }
    }
}
