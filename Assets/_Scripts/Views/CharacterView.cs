using Quantum;
using UnityEngine;
using Input = UnityEngine.Input;

public class CharacterView : QuantumEntityViewComponent
{
    private bool _isLocalPlayer;
    
    public override void OnActivate(Frame frame)
    {
        
        if (frame.TryGet<PlayerLink>(EntityRef, out var playerLink))
        {
            _isLocalPlayer = Game.PlayerIsLocal(playerLink.playerRef);
        }
    }
    
    
}
