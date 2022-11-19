using System.Collections.Generic;
using ExplogineCore;
using ExplogineCore.Data;
using ExplogineMonoGame;
using ExplogineMonoGame.Cartridges;
using ExplogineMonoGame.HitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SQJ22;

public class SqGameCartridge : BasicGameCartridge
{
    private EntityData _e1;
    private GridSpaceRenderer _gridRenderer;
    private GridHoverer _hoverer;
    private GridSpace _space;
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
        _space = new GridSpace(10, 10);
        _hoverer = new GridHoverer();

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
            _space.AttemptMoveEntity(_e1, new Point(1, 0));
        }

        if (Client.Input.Keyboard.GetButton(Keys.Left).WasPressed)
        {
            _space.AttemptMoveEntity(_e1, new Point(-1, 0));
        }

        if (Client.Input.Keyboard.GetButton(Keys.Up).WasPressed)
        {
            _space.AttemptMoveEntity(_e1, new Point(0, -1));
        }

        if (Client.Input.Keyboard.GetButton(Keys.Down).WasPressed)
        {
            _space.AttemptMoveEntity(_e1, new Point(0, 1));
        }

        var hitTestStack = new HitTestStack();

        _hoverer.UpdateHitTest(_space, _gridRenderer.Settings, hitTestStack);
        hitTestStack.Resolve(Client.Input.Mouse.Position(Client.RenderCanvas.ScreenToCanvas));
        _hoverer.UpdateInteraction(_space, _gridRenderer.Settings);
    }

    public override void Draw(Painter painter)
    {
        painter.BeginSpriteBatch(SamplerState.LinearWrap);
        _gridRenderer.DrawEntities(painter, _space, Depth.Middle);
        _gridRenderer.DrawSpace(painter, _space, Depth.Middle + 100);

        var mousePosCell = _gridRenderer.Settings.GetGridPositionFromWorldPosition(
            Client.Input.Mouse.Position(Client.RenderCanvas.ScreenToCanvas));

        if (_hoverer.HasGrabbed)
        {
            foreach (var cell in _hoverer.Grabbed.Body.Cells())
            {
                _gridRenderer.HighlightCell(painter, _space, cell + mousePosCell /*+ grabOffset*/, Depth.Middle - 200);
            }
        }
        else
        {
            _gridRenderer.HighlightCell(painter, _space, mousePosCell, Depth.Middle - 200);
        }

        painter.EndSpriteBatch();
    }
}
