using System;
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
using Microsoft.Xna.Framework.Input;

namespace SQJ22;

public class SqGameCartridge : BasicGameCartridge
{
    private GridHoverer _hoverer;
    private GridSpaceRenderer _spaceRenderer;
    private bool IsInShop => ServiceLocator.Locate<Battle>().InternalEncounter is ShopEncounter;
    private GridSpace Space => ServiceLocator.Locate<GridSpace>();

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
        ServiceLocator.Register(new GridSpace(10, 10));

        _spaceRenderer = new GridSpaceRenderer(new(new Vector2(300, 400), 64));
        _hoverer = new GridHoverer();

        Space.AddEntityFromData(
            EntityDataLibrary.Rune,
            new Point(1, 1),
            Direction.None
        );

        Space.AddEntityFromData(
            EntityDataLibrary.Glider,
            new Point(2, 2),
            Direction.Right
        );

        Space.AddEntityFromData(
            EntityDataLibrary.Crystal,
            new Point(3, 5),
            Direction.Down
        );

        Space.AddEntityFromData(
            EntityDataLibrary.E1,
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
        _hoverer.UpdateHitTest(Space, _spaceRenderer.Settings, hitTestStack);
        hitTestStack.Resolve(Client.Input.Mouse.Position(Client.RenderCanvas.ScreenToCanvas));

        if (IsInShop)
        {
            _hoverer.UpdateDrag(Space, _spaceRenderer.Settings);
        }
        else
        {
            if (_hoverer.PollForTap())
            {
                var encounter = ServiceLocator.Locate<Battle>().BattleEncounter;
                encounter.PlayerMove.LoseOneEnergy();

                if (encounter.PlayerMove.IsOutOfEnergy())
                {
                    encounter.ExecutePlayerAndEnemyTurn();
                }
            }
        }
    }

    public override void Draw(Painter painter)
    {
        painter.BeginSpriteBatch(SamplerState.LinearWrap);
        var entityDepth = Depth.Middle;
        var slightlyAboveEntityDepth = entityDepth - 10;
        var spaceDepth = entityDepth + 100;
        var overlayDepth = entityDepth - 200;
        var statusEffectDepth = overlayDepth + 10;
        var previewDepth = overlayDepth + 20;

        var battleEncounter = ServiceLocator.Locate<Battle>().BattleEncounter;

        _spaceRenderer.DrawEntities(painter, Space, entityDepth);
        _spaceRenderer.DrawSpace(painter, Space, spaceDepth);
        if (battleEncounter != null)
        {
            _spaceRenderer.DrawStatusEffects(painter, battleEncounter.StatusEffects, _spaceRenderer.Settings,
                statusEffectDepth);
        }

        var mousePosCell = _spaceRenderer.Settings.GetGridPositionFromWorldPosition(
            Client.Input.Mouse.Position(Client.RenderCanvas.ScreenToCanvas));

        if (_hoverer.HasGrabbed)
        {
            foreach (var cell in _hoverer.Grabbed.Data.Body.Cells())
            {
                _spaceRenderer.HighlightCell(painter, Space, cell + mousePosCell - _hoverer.Grabbed.Offset,
                    slightlyAboveEntityDepth);
            }
        }
        else
        {
            // _spaceRenderer.HighlightCell(painter, _space, mousePosCell, overlayDepth);

            if (_hoverer.HasHoveredEntity())
            {
                var entity = _hoverer.GetHoveredEntity();
                if (entity.Direction != Direction.None)
                {
                    // Draw arrows
                    var arrows = GetArrowTexture(entity.Direction);
                    DrawMacros.DrawOverlayTextureOnEntity(painter, entity, _spaceRenderer.Settings, arrows,
                        entity.Direction.ToVector2() * 60f, overlayDepth);
                }
                else
                {
                    DrawMacros.DrawOverlayTextureOnEntity(painter, entity, _spaceRenderer.Settings,
                        Client.Assets.GetTexture("selection"),
                        new Vector2(-20, -20), overlayDepth);
                }
            }
        }

        if (battleEncounter != null)
        {
            battleEncounter.EnemyMove.CurrentAttack?.DrawPreview(painter, _spaceRenderer.Settings, previewDepth);

            if (battleEncounter.EnemyMove.CurrentAttack != null)
            {
                painter.DrawStringAtPosition(Client.Assets.GetFont("GameFont", 70),
                    $"Enemy is going to: {battleEncounter.EnemyMove.CurrentAttack.Description()}\n" +
                    $"Player is going to deal {battleEncounter.PlayerMove.PendingDamage} damage\n" +
                    $"Player has {battleEncounter.PlayerMove.Energy} energy\n" +
                    $"Player has {battleEncounter.PlayerAgent.Health} HP\n" +
                    $"Enemy has {battleEncounter.EnemyAgent.Health} HP",
                    new Point(10, 10),
                    new DrawSettings {Color = Color.White, Depth = overlayDepth});
            }

            painter.DrawAtPosition(battleEncounter.MonsterImage,
                new Vector2(1300, 100 + MathF.Sin(ServiceLocator.Locate<RuntimeClock>().ElapsedTime) * 20), 
                new Scale2D(0.5f),
                new DrawSettings{Flip = new XyBool(true, false)});
        }

        if (IsInShop)
        {
            painter.DrawStringAtPosition(Client.Assets.GetFont("GameFont", 70),
                "SHOP\nPress Enter to start next fight",
                new Point(10, 10),
                new DrawSettings {Color = Color.White, Depth = overlayDepth});

            if (Client.Input.Keyboard.GetButton(Keys.Enter).WasPressed)
            {
                ServiceLocator.Locate<Battle>().StartNextBattle();
            }
        }

        painter.EndSpriteBatch();
    }

    private Texture2D GetArrowTexture(Direction direction)
    {
        return Client.Assets.GetAsset<TextureAsset>(direction.Name + "Arrows").Texture;
    }
}
