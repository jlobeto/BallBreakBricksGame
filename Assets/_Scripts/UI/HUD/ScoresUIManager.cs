using System;
using System.Collections.Generic;
using Quantum;
using TMPro;
using UnityEngine;

public class ScoresUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text[] scores;
    
    void Start()
    {
        QuantumEvent.Subscribe<EventOnScoreUpdate>(this, OnScoreUpdate);
    }

    private void OnDestroy()
    {
        QuantumEvent.UnsubscribeListener(this);
    }

    private void OnScoreUpdate(EventOnScoreUpdate data)
    {
        if (data.playerRef > scores.Length) return;
        if (data.playerRef < 0) return;

        var tmp = scores[data.playerRef];
        tmp.DoNumberTextAnimAsync(data.newScore, 0.5f);
    }
}
