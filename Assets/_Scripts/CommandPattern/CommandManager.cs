using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CommandManager
{
    private List<ICommand> commands = new();

    public void AddCommand(ICommand command)
    {
        commands.Add(command);
    }
    
    public void AddCommands(List<ICommand> commands)
    {
        commands.AddRange(commands);
    }

    public async UniTask ExecuteCommands()
    {
        foreach (var command in commands)
        {
            await command.Execute();
        }
    }
}

public interface ICommand
{
    UniTask Execute();
}
