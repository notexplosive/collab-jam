using ExplogineCore.Data;
using ExplogineMonoGame;
using ExplogineMonoGame.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SQJ22;

public static class DrawMacros
{
    public static void DrawOverlayTextureOnEntity(Painter painter, Entity entity, RenderSettings renderSettings,
        Texture2D overlayTexture, Vector2 scrollAmount, Depth depth)
    {
        foreach (var cellPosition in entity.CellPositions())
        {
            DrawMacros.DrawOverlayTextureOnCell(painter, cellPosition.Global, renderSettings, overlayTexture,
                scrollAmount, depth);
        }
    }

    public static void DrawOverlayTextureOnCell(Painter painter, Point cellPosition, RenderSettings renderSettings,
        Texture2D overlayTexture, Vector2 scrollAmount, Depth depth)
    {
        var destinationRect = new Rectangle(
            renderSettings.CellPositionToRenderedPosition(cellPosition).ToPoint(),
            renderSettings.CellSizeAsPoint);

        var sourceRect = destinationRect;
        sourceRect.Location +=
            (-ServiceLocator.Locate<RuntimeClock>().ElapsedTime * scrollAmount).ToPoint();

        painter.DrawAsRectangle(overlayTexture, destinationRect,
            new DrawSettings
            {
                SourceRectangle = sourceRect,
                Color = Color.White.WithMultipliedOpacity(0.5f),
                Depth = depth
            });
    }
}
