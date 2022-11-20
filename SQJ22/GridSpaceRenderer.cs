using System;
using ExplogineCore.Data;
using ExplogineMonoGame;
using ExplogineMonoGame.Data;
using Microsoft.Xna.Framework;
using Vector2 = System.Numerics.Vector2;

namespace SQJ22;

public class GridSpaceRenderer
{
    public RenderSettings Settings { get; } = new(new Vector2(800, 100), 32);

    public void DrawEntities(Painter painter, GridSpace space, Depth depth)
    {
        foreach (var entity in space.Entities())
        {
            entity.Data.Renderer.DrawEntity(painter, Settings, entity, depth);
        }
    }

    public void DrawSpace(Painter painter, GridSpace space, Depth depth)
    {
        for (var x = 0; x < space.Size.X; x++)
        {
            for (var y = 0; y < space.Size.Y; y++)
            {
                // debug, just draw rectangles
                var color = (x % 2 == 1 && y % 2 == 0) || (x % 2 == 0 && y % 2 == 1)
                    ? Color.Teal.WithMultipliedOpacity(0.25f)
                    : Color.Teal.WithMultipliedOpacity(0.10f);
                painter.DrawRectangle(
                    new Rectangle(Settings.CellPositionToRenderedPosition(new Point(x, y)).ToPoint(),
                        Settings.CellSizeAsPoint), new DrawSettings {Depth = depth, Color = color});
            }
        }
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
                DrawMacros.DrawOverlayTextureOnCell(painter, cell, settings, statusEffect.Template.Texture, Vector2.Zero, depth);
            }
        }
    }
}
