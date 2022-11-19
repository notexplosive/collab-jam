using System;
using ExplogineCore.Data;
using ExplogineMonoGame.HitTesting;
using Microsoft.Xna.Framework;

namespace SQJ22;

public class GridHoverer
{
    private Entity? _hoveredEntity;

    public void Update(GridSpace space, RenderSettings settings, HitTestStack hitTestStack)
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
}
