using Photon.Client;
using Photon.Realtime;
using Quantum;
using TMPro;
using UnityEngine;

public class PlayerNamesUI : MonoBehaviour
{
    [SerializeField] private TMP_Text leftPlayerName;
    [SerializeField] private TMP_Text rightPlayerName;
    void Awake()
    {
        QuantumEvent.Subscribe<EventOnPlayerAdded>(this, OnPlayerAdded);
    }

    private void OnPlayerAdded(EventOnPlayerAdded data)
    {
        var player = data.Game.Frames.Verified.GetPlayerData(data.playerRef);
        if(data.playerRef == 0)
            leftPlayerName.text = player.PlayerNickname;
        else 
            rightPlayerName.text = player.PlayerNickname;
        
    }

}
