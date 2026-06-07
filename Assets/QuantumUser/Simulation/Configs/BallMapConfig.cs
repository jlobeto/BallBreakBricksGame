using Photon.Deterministic;
using UnityEngine;

namespace Quantum
{
    public class BallMapConfig : AssetObject
    {
        public FP maxYPos = 20;
        public FP deadZoneYPos = -7;
        
        public FP minXPos = -20;
        public FP maxXPos = 7;
    }
}
