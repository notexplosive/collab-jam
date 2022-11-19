using System;
using ExplogineCore.Data;
using ExplogineMonoGame;
using ExplogineMonoGame.HitTesting;
using ExplogineMonoGame.Input;
using Microsoft.Xna.Framework;

namespace SQJ22;

public class GridHoverer
{
    private Entity? _grabbedEntity;
    private Entity? _hoveredEntity;

    public bool HasGrabbed => _grabbedEntity.HasValue;

    public EntityData Grabbed =>
        _grabbedEntity.HasValue ? _grabbedEntity.Value.Data : throw new Exception("Nothing grabbed");

    public void UpdateHitTest(GridSpace space, RenderSettings settings, HitTestStack hitTestStack)
    {
        _hoveredEntity = null;
        foreach (var entity in space.Entities())
        {
            foreach (var cellPosition in entity.CellPositions())
            {
                hitTestStack.Add(
                    new Rectangle(
                        settings.CellPositionToRenderedPosition(cellPosition).ToPoint(),
                        settings.CellSizeAsPoint),
                    Depth.Middle,
                    () => { _hoveredEntity = entity; });
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

    public void UpdateInteraction(GridSpace space, RenderSettings renderSettings)
    {
        if (HasHoveredEntity())
        {
            if (Client.Input.Mouse.GetButton(MouseButton.Left).WasPressed)
            {
                var grabbed = GetHoveredEntity();
                space.RemoveEntity(grabbed);
                _grabbedEntity = grabbed;
            }
        }

        if (Client.Input.Mouse.GetButton(MouseButton.Left).WasReleased)
        {
            if (_grabbedEntity.HasValue)
            {
                var dropped = _grabbedEntity.Value;
                var droppedPosition = renderSettings.GetGridPositionFromWorldPosition(
                    Client.Input.Mouse.Position(Client.RenderCanvas.ScreenToCanvas));
                if (space.CanAddEntity(dropped.Data, droppedPosition))
                {
                    space.AddEntityFromData(dropped.Data, droppedPosition);
                }
                else
                {
                    space.AddEntity(dropped);
                }

                _grabbedEntity = null;
            }
        }
    }
}
