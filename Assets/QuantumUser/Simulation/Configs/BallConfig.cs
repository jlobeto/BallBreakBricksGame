namespace Quantum
{
    using Photon.Deterministic;

    public class BallConfig : AssetObject
    {
        public AssetRef<EntityPrototype> BallPrototype;
        public FP InitialSpeed;
    }
}
