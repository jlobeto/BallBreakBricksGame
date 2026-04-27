using Quantum;
using UnityEngine;
using Input = UnityEngine.Input;

public class CharacterView : QuantumEntityViewComponent
{
    private bool _thrownFirstBall;
    
    public override void OnActivate(Frame frame)
    {
        
    }

    public override void OnUpdateView()
    {
        if (!_thrownFirstBall && Input.GetKeyDown(KeyCode.Space))
        {
            //_thrownFirstBall = true;
            QuantumRunner.Default.Game.SendCommand(new ThrowBallCommand());
        }
    }
}
