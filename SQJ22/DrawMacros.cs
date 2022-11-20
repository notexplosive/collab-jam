using ExplogineCore.Data;
using ExplogineMonoGame;
using ExplogineMonoGame.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SQJ22;

public static class DrawMacros
{
    public static void DrawOverlayTextureOnEntity(Painter painter, Entity entity, RenderSettings renderSettings,
        Texture2D overlayTexture, Vector2 scrollAmount)
    {
        foreach (var cellPosition in entity.CellPositions())
        {
            var destinationRect = new Rectangle(
                renderSettings.CellPositionToRenderedPosition(cellPosition.Global).ToPoint(),
                renderSettings.CellSizeAsPoint);

            var sourceRect = destinationRect;
            sourceRect.Location -=
                (ServiceLocator.Locate<RuntimeClock>().ElapsedTime * scrollAmount).ToPoint();

            painter.DrawAsRectangle(overlayTexture, destinationRect,
                new DrawSettings
                {
                    SourceRectangle = sourceRect,
                    Color = Color.White.WithMultipliedOpacity(0.5f),
                    Depth = Depth.Middle - 250
                });
        }
    }
}
