using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SQJ22;

public readonly record struct Entity(GridSpace Space, EntityData Data, Point Position)
{
    public IEnumerable<CellPosition> CellPositions()
    {
        foreach (var localPosition in Data.Body.Cells())
        {
            yield return new CellPosition(localPosition, localPosition + Position);
        }
    }

    public record struct CellPosition(Point Local, Point Global);

    public void AttemptMove(Point offset)
    {
        var newEntity = this with {Position = Position + offset};
        Space.RemoveEntity(this);
        var canMove = true;
        
        foreach (var newCellPosition in newEntity.CellPositions())
        {
            if (Space.HasEntityAt(newCellPosition.Global))
            {
                canMove = false;
                var nudgedEntityData = Space.GetEntityDataAt(newCellPosition.Global);
                nudgedEntityData.Behavior.Nudged.Execute(this);
            }

            if (!Space.ContainsCell(newCellPosition.Global))
            {
                canMove = false;
            }
        }
        
        if (canMove)
        {
            Space.AddEntity(newEntity);
            Data.Behavior.Moved.Execute(this);
        }
        else
        {
            Space.AddEntity(this);
            Data.Behavior.Blocked.Execute(this);
        }
    }

    public void Tap()
    {
        foreach (var entity in GetAdjacentEntities())
        {
            entity.AdjacentTapped();
        }
        
        Data.Behavior.Tapped.Execute(this);
    }

    private void AdjacentTapped()
    {
        Data.Behavior.AdjacentTapped.Execute(this);
    }

    private IEnumerable<Entity> GetAdjacentEntities()
    {
        var adjacentEntities = new HashSet<Entity>();

        foreach (var cellPosition in CellPositions())
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    var entity = Space.GetEntityAt(cellPosition.Global);

                    if (entity.HasValue && entity != this && !adjacentEntities.Contains(entity.Value))
                    {
                        adjacentEntities.Add(entity.Value);
                    }
                }
            }
        }

        return adjacentEntities;
    }
}
