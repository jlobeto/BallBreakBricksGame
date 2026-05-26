using Cysharp.Threading.Tasks;
using UnityEngine;

public class WaitForSecondsCommand : ICommand
{
    private float _seconds;
    public WaitForSecondsCommand(float seconds)
    {
        _seconds = seconds;
    }

    public async UniTask Execute()
    {
        await UniTask.WaitForSeconds(_seconds);
    }
}
