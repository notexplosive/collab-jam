using System;
using ExplogineCore.Data;
using ExplogineMonoGame;
using ExplogineMonoGame.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SQJ22;

public class SpriteRenderer : IRenderer
{
    private readonly Lazy<Texture2D> _texture;

    public SpriteRenderer(string textureName)
    {
        _texture = new Lazy<Texture2D>(()=>Client.Assets.GetTexture(textureName));
    }

    public void DrawEntity(Painter painter, RenderSettings renderSettings, Entity entity, Depth depth)
    {
        var texture = _texture.Value;
        var position = renderSettings.CellPositionToRenderedPosition(entity.Position);
        position += renderSettings.ScaleFromGridToRendered(entity.Data.RenderHandle.Offset.Value);
        painter.DrawAtPosition(texture, position, Scale2D.One, new DrawSettings {Color = Color.White, Depth = depth});
    }
}
