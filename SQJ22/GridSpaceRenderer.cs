using ExplogineCore.Data;
using ExplogineMonoGame;
using ExplogineMonoGame.Data;
using Microsoft.Xna.Framework;
using Vector2 = System.Numerics.Vector2;

namespace SQJ22;

public class GridSpaceRenderer
{
    public RenderSettings Settings { get; } = new(new Vector2(800, 100), 64);

    public void DrawEntities(Painter painter, GridSpace space, Depth depth)
    {
        foreach (var entity in space.Entities())
        {
            entity.Data.Renderer.DrawEntity(painter, Settings, entity, depth);
        }
    }

    public void DrawSpace(Painter painter, GridSpace space, Depth depth)
    {
        var rectSize = space.Size.ToVector2() * Settings.CellSize;
        var asset = Client.Assets.GetTexture("grid");
        painter.DrawAsRectangle(asset,
            new Rectangle(Settings.CellPositionToRenderedPosition(Point.Zero).ToPoint(), rectSize.ToPoint()),
            new DrawSettings {Color = Color.White, Depth = depth, SourceRectangle = new Rectangle(Point.Zero, rectSize.ToPoint())});
    }

    public void HighlightCell(Painter painter, GridSpace space, Point cellPosition, Depth depth)
    {
        if (space.ContainsCell(cellPosition))
        {
            painter.DrawRectangle(
                new Rectangle(Settings.CellPositionToRenderedPosition(cellPosition).ToPoint(),
                    Settings.CellSizeAsPoint),
                new DrawSettings {Depth = depth, Color = Color.Yellow.WithMultipliedOpacity(0.5f)});
        }
    }

    public void DrawStatusEffects(Painter painter, StatusEffects statusEffects, RenderSettings settings, Depth depth)
    {
        foreach (var statusEffect in statusEffects.Instances())
        {
            foreach (var cell in statusEffect.Target.CellPositions())
            {
                DrawMacros.DrawOverlayTextureOnCell(painter, cell, settings, statusEffect.Template.Texture,
                    Vector2.Zero, depth);
            }
        }
    }
}
