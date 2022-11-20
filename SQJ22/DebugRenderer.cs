using ExplogineCore.Data;
using ExplogineMonoGame;
using ExplogineMonoGame.Data;
using Microsoft.Xna.Framework;

namespace SQJ22;

public class DebugRenderer : IRenderer
{
    private readonly Color _color;

    public DebugRenderer()
    {
        var colorInt = Client.Random.Dirty.NextUInt() & 0xffffff;
        _color = new Color(colorInt | 0xff000000);
    }

    public void DrawEntity(Painter painter, RenderSettings renderSettings, Entity entity, Depth depth)
    {
        var rectSize = new Point(renderSettings.CellSize);

        foreach (var cellPosition in entity.CellPositions())
        {
            var rectPos = renderSettings.CellPositionToRenderedPosition(cellPosition.Global).ToPoint();
            var rectangle = new Rectangle(rectPos, rectSize);

            rectangle.Location +=
                renderSettings.ScaleFromGridToRendered(entity.Data.RenderHandle.Offset.Value).ToPoint();

            painter.DrawRectangle(
                rectangle,
                new DrawSettings {Color = _color, Depth = depth});
        }
    }
}