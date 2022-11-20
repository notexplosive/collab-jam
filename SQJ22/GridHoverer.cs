using System;
using ExplogineCore.Data;
using ExplogineMonoGame;
using ExplogineMonoGame.HitTesting;
using ExplogineMonoGame.Input;
using Microsoft.Xna.Framework;

namespace SQJ22;

public class GridHoverer
{
    private GrabState _grabState = GrabState.Empty;
    private HoverState _hoverState;

    public bool HasGrabbed => _grabState.IsNotEmpty;

    public GrabState Grabbed =>
        _grabState.IsNotEmpty ? _grabState : throw new Exception("Nothing grabbed");

    public void UpdateHitTest(GridSpace space, RenderSettings settings, HitTestStack hitTestStack)
    {
        _hoverState = HoverState.Empty;
        foreach (var entity in space.Entities())
        {
            foreach (var cellPosition in entity.CellPositions())
            {
                hitTestStack.Add(
                    new Rectangle(
                        settings.CellPositionToRenderedPosition(cellPosition.Global).ToPoint(),
                        settings.CellSizeAsPoint),
                    Depth.Middle,
                    () => { _hoverState = new HoverState(entity, cellPosition.Local); });
            }
        }
    }

    public bool HasHoveredEntity()
    {
        return _hoverState.IsNotEmpty;
    }

    public Entity GetHoveredEntity()
    {
        if (_hoverState.IsEmpty)
        {
            throw new Exception("No hovered entity");
        }

        return _hoverState.Entity;
    }

    public void UpdateDrag(GridSpace space, RenderSettings renderSettings)
    {
        if (HasHoveredEntity())
        {
            if (Client.Input.Mouse.GetButton(MouseButton.Left).WasPressed)
            {
                var grabbed = GetHoveredEntity();
                space.RemoveEntity(grabbed);
                _grabState = new GrabState(grabbed, _hoverState.HoveredCell);
            }
        }

        if (Client.Input.Mouse.GetButton(MouseButton.Left).WasReleased)
        {
            if (_grabState.IsNotEmpty)
            {
                var dropped = _grabState.Entity;
                var droppedPosition = renderSettings.GetGridPositionFromWorldPosition(
                    Client.Input.Mouse.Position(Client.RenderCanvas.ScreenToCanvas)) - _grabState.Offset;
                if (space.CanAddEntity(dropped.Data, droppedPosition))
                {
                    space.AddEntityFromData(dropped.Data, droppedPosition, dropped.Direction);
                }
                else
                {
                    space.AddEntity(dropped);
                }

                _grabState = GrabState.Empty;
            }
        }
    }

    public bool PollForTap()
    {
        if (_hoverState.IsNotEmpty)
        {
            if (Client.Input.Mouse.GetButton(MouseButton.Left).WasPressed)
            {
                _hoverState.Entity.Tap();
                return true;
            }
        }

        return false;
    }

    public readonly record struct HoverState(Entity? MaybeEntity, Point HoveredCell)
    {
        public static HoverState Empty = new(null, Point.Zero);
        public bool IsNotEmpty => !IsEmpty;
        public bool IsEmpty => this == HoverState.Empty || ServiceLocator.Locate<Animation>().IsPlaying();
        public Entity Entity => MaybeEntity ?? throw new Exception("HoverState was empty");
    }

    public readonly record struct GrabState(Entity? MaybeEntity, Point Offset)
    {
        public static GrabState Empty = new(null, Point.Zero);
        public bool IsNotEmpty => this != GrabState.Empty;

        public EntityData Data =>
            MaybeEntity != null ? MaybeEntity.Value.Data : throw new Exception("GrabState was empty");

        public Entity Entity => MaybeEntity ?? throw new Exception("GrabState was empty");
    }
}
