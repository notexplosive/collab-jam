using ExplogineMonoGame;

namespace SQJ22;

public class LogConsoleAction : ITokenAction
{
    private readonly string _message;

    public LogConsoleAction(string message)
    {
        _message = message;
    }

    public void Execute(Entity entity)
    {
        Client.Debug.Log(_message);
    }
}
