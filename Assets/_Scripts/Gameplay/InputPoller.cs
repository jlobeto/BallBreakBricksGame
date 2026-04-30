using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using Quantum;
using UnityEngine;
using Input = UnityEngine.Input;

public class InputPoller : MonoBehaviour
{
    [SerializeField] private List<LocalPlayerInput> inputs;
    
    private Dictionary<int, LocalPlayerInput> _localPlayersInputs = new ();
    
    private void OnEnable() {
        QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
        QuantumCallback.Subscribe(this, (CallbackLocalPlayerAddConfirmed c) => OnLocalPlayerAddConfirmed(c));
        
    }

    private void OnLocalPlayerAddConfirmed(CallbackLocalPlayerAddConfirmed c)
    {
        _localPlayersInputs[c.PlayerSlot] = inputs[c.PlayerSlot];
    }

    private void PollInput(CallbackPollInput callback) 
    {
        Quantum.Input i = new Quantum.Input();

        var inputType = _localPlayersInputs[callback.PlayerSlot];
        
        if (Input.GetKey(inputType.MoveLeft))
            i.HorizontalInput = -1;
        else if(Input.GetKey(inputType.MoveRight))
            i.HorizontalInput = 1;
        else if (Input.GetKey(inputType.ThrowBall))
            i.ThrowBall = true;
        
        callback.SetInput(i, DeterministicInputFlags.Repeatable);
        
    }

}


[Serializable]
public class LocalPlayerInput
{
    public KeyCode MoveLeft = KeyCode.A;
    public KeyCode MoveRight = KeyCode.D;
    public KeyCode ThrowBall = KeyCode.Space;
}
