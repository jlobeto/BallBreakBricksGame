using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using Quantum;
using UnityEngine;
using Input = UnityEngine.Input;

public class InputPoller : MonoBehaviour
{
    private void OnEnable() {
        QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
    }

    private void PollInput(CallbackPollInput callback) 
    {
        Quantum.Input i = new Quantum.Input();

        i.HorizontalInput = Input.GetAxis("Horizontal").ToFP();
        
        callback.SetInput(i, DeterministicInputFlags.Repeatable);
    }
}
