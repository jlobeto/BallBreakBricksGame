using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private UIDocument menuUIDocument;
    
    private VisualElement _root;
    private TextField _userNameField;

    private void Start()
    {
        _root = menuUIDocument.rootVisualElement;
        
        _userNameField = _root.Q<TextField>("userName");
        _userNameField.SetValueWithoutNotify(PlayerDataManager.Instance.UserName);
        
        var offlineBtn = _root.Q<Button>("offlineBtn");
        var onlineBtn = _root.Q<Button>("onlineBtn");
        
        offlineBtn.RegisterCallback<ClickEvent>(OnOfflineClicked);
        onlineBtn.RegisterCallback<ClickEvent>(OnOnlineClicked);
    }

    private void OnOnlineClicked(ClickEvent evt)
    {
        SavePlayerName();
    }

    private void OnOfflineClicked(ClickEvent evt)
    {
        Debug.Log("Offline clicked");
        SavePlayerName();
        WaitForLoadingScreen().Forget();
    }

    private void SavePlayerName()
    {
        PlayerDataManager.Instance.SavePlayerName(_userNameField?.value);
    }

    private async UniTask WaitForLoadingScreen()
    {
        await GameManager.Instance.GameModeSelected(false);
        await UniTask.WaitForSeconds(0.1f);
        _root.style.display = DisplayStyle.None;
    }
}
