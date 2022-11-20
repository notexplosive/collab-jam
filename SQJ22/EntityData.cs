namespace SQJ22;

public class EntityData
{
    public EntityData(EntityName name, Grid body, TokenBehavior behavior, IRenderer renderer)
    {
        Name = name;
        Body = body;
        Behavior = behavior;
        Renderer = renderer;
        RenderHandle = new RenderHandle();
    }

    public TokenBehavior Behavior { get; }
    public EntityName Name { get; }
    public Grid Body { get; }
    public IRenderer Renderer { get; }
    public RenderHandle RenderHandle { get; }
}