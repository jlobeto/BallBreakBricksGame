using Quantum;

public class CharacterView : QuantumEntityViewComponent
{
    private bool _isLocalPlayer;
    
    public override void OnActivate(Frame frame)
    {
        
        if (frame.TryGet<Quantum.PlayerData>(EntityRef, out var playerLink))
        {
            _isLocalPlayer = Game.PlayerIsLocal(playerLink.playerRef);
        }
    }
    
    
}
