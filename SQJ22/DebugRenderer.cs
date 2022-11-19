﻿using ExplogineMonoGame;
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
    
    public void Draw(Painter painter, RenderSettings renderSettings, Entity entity)
    {
        var rectSize = new Point(renderSettings.CellSize);
        
        foreach (var cellPosition in entity.CellPositions())
        {
            var rectPos = cellPosition.ToVector2() * renderSettings.CellSize + renderSettings.Position;
            painter.DrawRectangle(
                new Rectangle(
                    rectPos.ToPoint(),
                    rectSize),
                new DrawSettings {Color = _color});
        }
    }
}