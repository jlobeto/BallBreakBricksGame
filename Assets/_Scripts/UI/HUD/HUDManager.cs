using Cysharp.Threading.Tasks;
using Quantum;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private UserControlsUI userControlsUI;
    [SerializeField] private float transitionDurations = 1;
    void Start()
    {
        QuantumEvent.Subscribe<EventOnPlayerAdded>(this, OnPlayerAdded);
    }

    private void OnPlayerAdded(EventOnPlayerAdded data)
    {
        QuantumEvent.UnsubscribeListener<EventOnPlayerAdded>(this);

        HandleUIInitialization().Forget();
    }

    private async UniTask HandleUIInitialization()
    {
        var commandMan = new CommandManager();
        commandMan.AddCommand(new WaitForSecondsCommand(transitionDurations));
        commandMan.AddCommand(userControlsUI);
        await commandMan.ExecuteCommands();
    }
}
