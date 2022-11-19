namespace SQJ22;

public class EntityData
{
    public EntityData(Grid body, IRenderer renderer)
    {
        Body = body;
        Renderer = renderer;
    }

    public Grid Body { get; }
    public IRenderer Renderer { get; }
}
