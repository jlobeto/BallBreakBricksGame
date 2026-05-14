using System;
using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text loadingTitle;
    
    void Start()
    {
        EventBus.AddListener<EventOnLoadingChanged>(OnEventLoadingChanged);
        EventBus.AddListener<EventOnQuantumGameStarted>(OnQuantumGameStarted);
        DontDestroyOnLoad(gameObject);
    }

    private void OnQuantumGameStarted(EventOnQuantumGameStarted obj)
    {
        Destroy(gameObject);
    }

    private void OnEventLoadingChanged(EventOnLoadingChanged data)
    {
        loadingTitle.text = data.loadingTitle;
    }

    private void OnDestroy()
    {
        EventBus.RemoveListener<EventOnLoadingChanged>(OnEventLoadingChanged);
        EventBus.RemoveListener<EventOnQuantumGameStarted>(OnQuantumGameStarted);
    }
}
