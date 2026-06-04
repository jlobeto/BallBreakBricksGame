using System;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField userNameField;
    [SerializeField] private TMP_InputField roomId;
    [SerializeField] private Button offlineBtn;
    [SerializeField] private Button onlineBtn;
    [SerializeField] private TMP_Text bestRegionText;
    

    private void Start()
    {
        if(!string.IsNullOrEmpty(PlayerDataManager.Instance.UserName))
            userNameField.text = PlayerDataManager.Instance.UserName;
        
        offlineBtn.onClick.RemoveAllListeners();
        offlineBtn.onClick.AddListener(OnOfflineClicked);
        
        onlineBtn.onClick.RemoveAllListeners();
        onlineBtn.onClick.AddListener(OnOnlineClicked);
        
        EventBus.AddListener<EventOnRegionReceived>(OnRegionSelected);
    }

    private void OnRegionSelected(EventOnRegionReceived region)
    {
        bestRegionText.text = $"Best Region: {region.regionCode} - {region.ping}ms";
    }

    private void OnOnlineClicked()
    {
        SavePlayerName();
        LoadScene(true);
    }

    private void OnOfflineClicked()
    {
        SavePlayerName();
        LoadScene(false);
    }

    private void SavePlayerName()
    {
        PlayerDataManager.Instance.SavePlayerName(userNameField.text);
    }

    private void LoadScene(bool isOnline)
    {
        EventBus.Publish<EventOnPlayClicked>(new EventOnPlayClicked()
        {
            IsOnlineMatch =  isOnline,
            RoomId = roomId.text
        });
    }

    private void OnDestroy()
    {
        offlineBtn?.onClick.RemoveAllListeners();
        onlineBtn?.onClick.RemoveAllListeners();
        EventBus.RemoveListener<EventOnRegionReceived>(OnRegionSelected);
    }

}
