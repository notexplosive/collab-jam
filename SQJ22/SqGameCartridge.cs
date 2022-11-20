using System.Collections.Generic;
using ExplogineCore;
using ExplogineCore.Data;
using ExplogineMonoGame;
using ExplogineMonoGame.AssetManagement;
using ExplogineMonoGame.Cartridges;
using ExplogineMonoGame.Data;
using ExplogineMonoGame.HitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SQJ22;

public class SqGameCartridge : BasicGameCartridge
{
    private readonly bool _isInShop = false;
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
        foreach (var direction in Direction.Each())
        {
            yield return new AssetLoadEvent($"{direction.Name}Arrows", () =>
            {
                var arrowTexture = Client.Assets.GetTexture("arrows");
                var canvas = new Canvas(arrowTexture.Width, arrowTexture.Height);
                Client.Graphics.PushCanvas(canvas);
                painter.BeginSpriteBatch(SamplerState.LinearWrap);
                painter.DrawAtPosition(arrowTexture, new Vector2(arrowTexture.Width, arrowTexture.Height) / 2,
                    Scale2D.One,
                    new DrawSettings {Angle = direction.ToAngle(), Origin = DrawOrigin.Center});
                painter.EndSpriteBatch();
                Client.Graphics.PopCanvas();
                return canvas.AsTextureAsset();
            });
        }
    }

    public override void OnCartridgeStarted()
    {
        ServiceLocator.Register(new Animation(false));
        ServiceLocator.Register(new Battle());
        ServiceLocator.Register(new RuntimeClock());

        _gridRenderer = new GridSpaceRenderer();
        _space = new GridSpace(10, 10);
        _hoverer = new GridHoverer();

        _space.AddEntityFromData(
            new EntityData(
                new Grid()
                    .AddCell(0, 0)
                    .AddCell(1, 0)
                    .AddCell(-1, 0)
                ,
                new TokenBehavior()
                    .OnTapped(
                        new DealDamageAction(5)
                    )
                ,
                new DebugRenderer()
            ),
            new Point(1, 1),
            Direction.None
        );

        _space.AddEntityFromData(
            new EntityData(
                new Grid()
                    .AddCell(0, 0)
                    .AddCell(1, 0)
                    .AddCell(0, 1)
                    .AddCell(1, 1)
                    .AddCell(1, 2)
                ,
                new TokenBehavior()
                    .OnTapped(
                        new MoveInFacingDirectionAction(),
                        new MoveInFacingDirectionAction(),
                        new RotateAction(Rotation.Clockwise)
                    )
                ,
                new DebugRenderer()
            ),
            new Point(2, 2),
            Direction.Right
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
                new TokenBehavior()
                    .OnTapped(
                        new MoveInFacingDirectionAction(),
                        new RotateAction(Rotation.CounterClockwise)
                    )
                ,
                new DebugRenderer()),
            new Point(5, 2),
            Direction.Down
        );
    }

    public override void Update(float dt)
    {
        ServiceLocator.Locate<RuntimeClock>().Update(dt);
        var animation = ServiceLocator.Locate<Animation>();
        animation.Update(dt);

        if (animation.IsPlaying())
        {
            return;
        }

        var hitTestStack = new HitTestStack();
        _hoverer.UpdateHitTest(_space, _gridRenderer.Settings, hitTestStack);
        hitTestStack.Resolve(Client.Input.Mouse.Position(Client.RenderCanvas.ScreenToCanvas));

        if (_isInShop)
        {
            _hoverer.UpdateDrag(_space, _gridRenderer.Settings);
        }
        else
        {
            if (_hoverer.PollForTap())
            {
                var battle = ServiceLocator.Locate<Battle>();
                battle.CurrentPlayerMove.LoseOneEnergy();

                if (battle.CurrentPlayerMove.IsOutOfEnergy())
                {
                    battle.ExecutePlayerAndEnemyTurn();
                    Client.Debug.Log("Player turn is over");
                }
                else
                {
                    Client.Debug.Log($"{battle.CurrentPlayerMove.Energy} energy remaining");
                }
            }
        }
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
            foreach (var cell in _hoverer.Grabbed.Data.Body.Cells())
            {
                _gridRenderer.HighlightCell(painter, _space, cell + mousePosCell - _hoverer.Grabbed.Offset,
                    Depth.Middle - 200);
            }
        }
        else
        {
            _gridRenderer.HighlightCell(painter, _space, mousePosCell, Depth.Middle - 200);

            // Draw arrows
            if (_hoverer.HasHoveredEntity())
            {
                var entity = _hoverer.GetHoveredEntity();
                if (entity.Direction != Direction.None)
                {
                    var arrows = GetArrowTexture(entity.Direction);
                    DrawMacros.DrawOverlayTextureOnEntity(painter, entity, _gridRenderer.Settings, arrows,
                        entity.Direction.ToVector2() * 60f);
                }
            }
        }

        painter.EndSpriteBatch();
    }

    private Texture2D GetArrowTexture(Direction direction)
    {
        return Client.Assets.GetAsset<TextureAsset>(direction.Name + "Arrows").Texture;
    }
}
