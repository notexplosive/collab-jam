namespace SQJ22;

public class EntityData
{
    public EntityData(Grid body, TokenBehavior behavior, IRenderer renderer)
    {
        Body = body;
        Behavior = behavior;
        Renderer = renderer;
    }

    public TokenBehavior Behavior { get; }
    public Grid Body { get; }
    public IRenderer Renderer { get; }
}