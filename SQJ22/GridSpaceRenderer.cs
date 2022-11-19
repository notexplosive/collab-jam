using ExplogineCore.Data;
using ExplogineMonoGame;
using ExplogineMonoGame.Data;
using Microsoft.Xna.Framework;
using Vector2 = System.Numerics.Vector2;

namespace SQJ22;

public class GridSpaceRenderer
{
    public RenderSettings Settings { get; } = new(new Vector2(100, 100), 32);

    public void DrawEntities(Painter painter, GridSpace space, Depth depth)
    {
        foreach (var entity in space.Entities())
        {
            entity.Data.Renderer.Draw(painter, Settings, entity, depth);
        }
    }

    public void DrawSpace(Painter painter, GridSpace space, Depth depth)
    {
        for (int x = 0; x < space.Size.X; x++)
        {
            for (int y = 0; y < space.Size.Y; y++)
            {
                // debug, just draw rectangles
                var color = (x % 2 == 1 && y % 2 == 0) || (x % 2 == 0 && y % 2 == 1) ? Color.White.WithMultipliedOpacity(0.25f) : Color.White.WithMultipliedOpacity(0.10f);
                painter.DrawRectangle(new Rectangle(Settings.CellPositionToRenderedPosition(new Point(x,y)).ToPoint(),Settings.CellSizeAsPoint), new DrawSettings{Depth = depth, Color = color});
            }
        }
    }
}