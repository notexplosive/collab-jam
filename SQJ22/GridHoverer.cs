using System;
using ExplogineCore.Data;
using ExplogineMonoGame;
using ExplogineMonoGame.HitTesting;
using ExplogineMonoGame.Input;
using Microsoft.Xna.Framework;

namespace SQJ22;

public class GridHoverer
{
    private GrabState _grabbedEntity = GrabState.Empty;
    private Entity? _hoveredEntity;
    private Point? _hoveredLocalCell;

    public bool HasGrabbed => _grabbedEntity.IsNotEmpty;

    public GrabState Grabbed =>
        _grabbedEntity.IsNotEmpty ? _grabbedEntity : throw new Exception("Nothing grabbed");

    public void UpdateHitTest(GridSpace space, RenderSettings settings, HitTestStack hitTestStack)
    {
        _hoveredEntity = null;
        _hoveredLocalCell = null;
        foreach (var entity in space.Entities())
        {
            foreach (var cellPosition in entity.CellPositions())
            {
                hitTestStack.Add(
                    new Rectangle(
                        settings.CellPositionToRenderedPosition(cellPosition.Global).ToPoint(),
                        settings.CellSizeAsPoint),
                    Depth.Middle,
                    () =>
                    {
                        _hoveredEntity = entity;
                        _hoveredLocalCell = cellPosition.Local;
                    });
            }
        }
    }

    public bool HasHoveredEntity()
    {
        return _hoveredEntity != null;
    }

    public Entity GetHoveredEntity()
    {
        if (_hoveredEntity == null)
        {
            throw new Exception("No hovered entity");
        }

        return _hoveredEntity.Value;
    }

    public void UpdateDrag(GridSpace space, RenderSettings renderSettings)
    {
        if (HasHoveredEntity())
        {
            if (Client.Input.Mouse.GetButton(MouseButton.Left).WasPressed)
            {
                var grabbed = GetHoveredEntity();
                space.RemoveEntity(grabbed);
                _grabbedEntity = new GrabState(grabbed, _hoveredLocalCell.Value);
            }
        }

        if (Client.Input.Mouse.GetButton(MouseButton.Left).WasReleased)
        {
            if (_grabbedEntity.IsNotEmpty)
            {
                var dropped = _grabbedEntity.Entity;
                var droppedPosition = renderSettings.GetGridPositionFromWorldPosition(
                    Client.Input.Mouse.Position(Client.RenderCanvas.ScreenToCanvas)) - _grabbedEntity.Offset;
                if (space.CanAddEntity(dropped.Data, droppedPosition))
                {
                    space.AddEntityFromData(dropped.Data, droppedPosition);
                }
                else
                {
                    space.AddEntity(dropped);
                }

                _grabbedEntity = GrabState.Empty;
            }
        }
    }

    public readonly record struct GrabState(Entity? MaybeEntity, Point Offset)
    {
        public static GrabState Empty = new(null, Point.Zero);
        public bool IsNotEmpty => this != GrabState.Empty;

        public EntityData Data =>
            MaybeEntity.HasValue ? MaybeEntity.Value.Data : throw new Exception("GrabState was empty");

        public Entity Entity => MaybeEntity.HasValue ? MaybeEntity.Value : throw new Exception("GrabState was empty");
    }
}
