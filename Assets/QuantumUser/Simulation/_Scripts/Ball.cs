using UnityEngine;

namespace Quantum
{
    public partial struct Ball
    {
        public void Initialize(BallConfig config)
        {
            damage = config.damage;
        }
    }
}
