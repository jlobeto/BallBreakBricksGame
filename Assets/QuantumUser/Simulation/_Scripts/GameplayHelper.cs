using UnityEngine;

namespace Quantum
{
    public static unsafe class GameplayHelper
    {
        public static PlayerRef GetEnemy(Frame f, PlayerRef myself)
        {
            foreach (var _ in f.Unsafe.GetComponentBlockIterator<PlayerLink>())
            {
                if(_.Component->playerRef == myself) continue;
                return _.Component->playerRef;
            }

            return default;
        }
    }
}
