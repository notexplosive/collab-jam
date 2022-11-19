using ExplogineMonoGame;
using ExTween;

namespace SQJ22;

public class LogConsoleAction : ITokenAction
{
    private readonly string _message;

    public LogConsoleAction(string message)
    {
        _message = message;
    }

    public ITween Execute(GridSpace space, EntityData data)
    {
        return new CallbackTween(() => Client.Debug.Log(_message));
    }
}
