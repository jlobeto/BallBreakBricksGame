using System;
using UnityEngine;

public class PlayerDataManager
{
    private string PlayerDataKey =>  $"PlayerData-{Application.dataPath.GetHashCode()}";
    private static PlayerDataManager _instance;
    public static PlayerDataManager Instance
    {
        get
        {
            if (_instance == null)
                return _instance = new PlayerDataManager();
            return _instance;
        }
    }

    public string UserName => _myPlayerData?.PlayerName;
    
    private PlayerData _myPlayerData;
    
    private PlayerDataManager()
    {
        var savedData = PlayerPrefs.GetString(PlayerDataKey, null);
        if(string.IsNullOrEmpty(savedData))
        {
            _myPlayerData = new PlayerData();
        }
        else
        {
            _myPlayerData = JsonUtility.FromJson<PlayerData>(savedData);
        }
    }

    public void SavePlayerName(string name)
    {
        if(!string.IsNullOrEmpty(name) && _myPlayerData.PlayerName != name)
        {
            _myPlayerData.PlayerName = name;
            Save();
        }
    }

    public void SaveTotalWins(int wins)
    {
        if (wins <= 0)
            return;
        if (_myPlayerData.TotalWins == wins)
            return;
        
        _myPlayerData.TotalWins = wins;
        Save();
    }

    public void SaveWinRate(int winRate)
    {
        if (winRate <= 0 || Mathf.Approximately(_myPlayerData.WinRate , winRate))
            return;
        
        _myPlayerData.WinRate = winRate;
        Save();
    }
    
    private void Save()
    {
        PlayerPrefs.SetString(PlayerDataKey, JsonUtility.ToJson(_myPlayerData));
        PlayerPrefs.Save();
    }
}

[Serializable]
public class PlayerData
{
    public int TotalWins;
    public string PlayerName;
    public float WinRate;
}
