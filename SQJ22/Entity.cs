using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SQJ22;

public readonly record struct Entity(GridSpace Space, EntityData Data, Point Position, Direction Direction)
{
    public IEnumerable<CellPosition> CellPositions()
    {
        foreach (var localPosition in Data.Body.Cells())
        {
            yield return new CellPosition(localPosition, localPosition + Position);
        }
    }

    public void Tap()
    {
        var animation = ServiceLocator.Locate<Animation>();
        animation.Enqueue(GameplayEvents.TriggerTap(this));
    }

    public IEnumerable<Entity> GetAdjacentEntities()
    {
        var adjacentEntities = new HashSet<Entity>();

        foreach (var cellPosition in CellPositions())
        {
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    var entity = Space.GetEntityAt(cellPosition.Global + new Point(x, y));

                    if (entity.HasValue && entity != this && !adjacentEntities.Contains(entity.Value))
                    {
                        adjacentEntities.Add(entity.Value);
                    }
                }
            }
        }

        return adjacentEntities;
    }

    public IEnumerable<CellPosition> ProjectedCells(Direction direction)
    {
        var set = new HashSet<CellPosition>();
        foreach (var cell in CellPositions())
        {
            var newCell = cell;

            while (Space.ContainsCell(newCell.Global))
            {
                newCell = new CellPosition(newCell.Local + direction.AsPoint, newCell.Local + direction.AsPoint + Position);
                if (!set.Contains(newCell) && !Data.Body.Contains(newCell.Local) && Space.ContainsCell(newCell.Global))
                {
                    set.Add(newCell);
                }
            }
        }

        return set;
    }

    public record struct CellPosition(Point Local, Point Global);
}
