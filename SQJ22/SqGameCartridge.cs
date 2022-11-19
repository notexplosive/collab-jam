using System.Collections.Generic;
using ExplogineCore;
using ExplogineMonoGame;
using ExplogineMonoGame.Cartridges;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SQJ22;

public class SqGameCartridge : BasicGameCartridge
{
    private GridSpaceRenderer _gridRenderer;
    private GridSpace _space;
    private EntityData _e1;
    public override CartridgeConfig CartridgeConfig { get; } = new(new Point(1920, 1080));

    public override void AddCommandLineParameters(CommandLineParametersWriter parameters)
    {
    }

    public override IEnumerable<ILoadEvent> LoadEvents(Painter painter)
    {
        yield return null;
    }

    public override void OnCartridgeStarted()
    {
        _gridRenderer = new GridSpaceRenderer();
        _space = new GridSpace();

        _space.AddEntityFromData(
            new EntityData(
                new Grid()
                    .AddCell(0, 0)
                    .AddCell(1, 0)
                    .AddCell(0, 1)
                    .AddCell(1, 1)
                    .AddCell(1, 2)
                ,
                new DebugRenderer()),
            new Point(2, 2)
        );

        _e1 = _space.AddEntityFromData(
            new EntityData(
                new Grid()
                    .AddCell(0, 0)
                    .AddCell(1, 0)
                    .AddCell(-1, 0)
                    .AddCell(0, 1)
                    .AddCell(0, -1)
                ,
                new DebugRenderer()),
            new Point(5, 2)
        );
    }

    public override void Update(float dt)
    {
        if (Client.Input.Keyboard.GetButton(Keys.Right).WasPressed)
        {
            _space.AttemptMoveEntity(_e1, new Point(1,0));
        }
        
        if (Client.Input.Keyboard.GetButton(Keys.Left).WasPressed)
        {
            _space.AttemptMoveEntity(_e1, new Point(-1,0));
        }
        
        if (Client.Input.Keyboard.GetButton(Keys.Up).WasPressed)
        {
            _space.AttemptMoveEntity(_e1, new Point(0,-1));
        }
        
        if (Client.Input.Keyboard.GetButton(Keys.Down).WasPressed)
        {
            _space.AttemptMoveEntity(_e1, new Point(0,1));
        }
    }

    public override void Draw(Painter painter)
    {
        painter.BeginSpriteBatch(SamplerState.LinearWrap);
        _gridRenderer.Draw(painter, _space);

        painter.EndSpriteBatch();
    }
}
