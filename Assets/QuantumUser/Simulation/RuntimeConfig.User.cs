namespace Quantum
{
    public partial class RuntimeConfig
    {
        public AssetRef<BallConfig> BallCommonConfig;
        public AssetRef<PlayerMapConfig> PlayerMapConfig;
        public AssetRef<ScoreSystemConfig> ScoreConfig;

        public int BlocksObjective = 15;
    }
}