using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private UIDocument menuUIDocument;
    
    public Action<bool, string> OnPlayModeClicked;
    
    private VisualElement _root;
    private TextField _userNameField;
    private Button _offlineBtn;
    private Button _onlineBtn;

    private void Start()
    {
        _root = menuUIDocument.rootVisualElement;
        
        _userNameField = _root.Q<TextField>("userName");
        if(!string.IsNullOrEmpty(PlayerDataManager.Instance.UserName))
            _userNameField.SetValueWithoutNotify(PlayerDataManager.Instance.UserName);
        
        _offlineBtn = _root.Q<Button>("offlineBtn");
        _onlineBtn = _root.Q<Button>("onlineBtn");
        
        _offlineBtn.RegisterCallback<ClickEvent>(OnOfflineClicked);
        _onlineBtn.RegisterCallback<ClickEvent>(OnOnlineClicked);
    }

    private void OnOnlineClicked(ClickEvent evt)
    {
        SavePlayerName();
        LoadScene(true);
    }

    private void OnOfflineClicked(ClickEvent evt)
    {
        SavePlayerName();
        LoadScene(false);
    }

    private void SavePlayerName()
    {
        PlayerDataManager.Instance.SavePlayerName(_userNameField?.value);
    }

    private void LoadScene(bool isOnline)
    {
        _root.style.display = DisplayStyle.None;
        
        var roomName = _root.Q<TextField>("roomName");
        OnPlayModeClicked?.Invoke(isOnline, roomName.value);
    }

    private void OnDestroy()
    {
        if(_offlineBtn != null)
            _offlineBtn.UnregisterCallback<ClickEvent>(OnOfflineClicked);
        if(_onlineBtn != null)
            _onlineBtn.UnregisterCallback<ClickEvent>(OnOnlineClicked);
    }
}
