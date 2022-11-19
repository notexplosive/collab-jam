using System.Numerics;
using ExplogineMonoGame;

namespace SQJ22;

public class GridSpaceRenderer
{
    public RenderSettings Settings { get; } = new(new Vector2(100, 100), 32);

    public void Draw(Painter painter, GridSpace space)
    {
        foreach (var entity in space.Entities())
        {
            entity.Data.Renderer.Draw(painter, Settings, entity);
        }
    }
}