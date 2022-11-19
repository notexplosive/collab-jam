using System;
using System.Collections.Generic;
using ExTween;
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

    public void AttemptMove(Point offset)
    {
        var newEntity = this with {Position = Position + offset};
        Space.RemoveEntity(this);
        var canMove = true;

        var nudgeSequence = new SequenceTween();
        var moveSequence = new SequenceTween();
        var blockSequence = new SequenceTween();

        
        foreach (var newCellPosition in newEntity.CellPositions())
        {
            if (Space.HasEntityAt(newCellPosition.Global))
            {
                canMove = false;
                var nudgedMaybeEntity = Space.GetEntityAt(newCellPosition.Global);
                if (!nudgedMaybeEntity.HasValue)
                {
                    throw new Exception("API violation, Space said we had an entity here, but we didn't");
                }

                var nudgedEntity = nudgedMaybeEntity.Value;
                nudgeSequence.Add(nudgedEntity.Data.Behavior.Nudged.Execute(this));
                nudgeSequence.Add(GameplayEvents.AnimateNudged(nudgedEntity.Data.RenderHandle, offset));
            }

            if (!Space.ContainsCell(newCellPosition.Global))
            {
                canMove = false;
            }
        }

        if (canMove)
        {
            Space.AddEntity(newEntity);
            moveSequence.Add(Data.Behavior.Moved.Execute(this));
            moveSequence.Add(GameplayEvents.AnimateMove(newEntity.Data.RenderHandle, offset));
        }
        else
        {
            Space.AddEntity(this);
            blockSequence.Add(Data.Behavior.Blocked.Execute(this));
            blockSequence.Add(GameplayEvents.AnimateBlock(Data.RenderHandle, offset));
        }
        
        var animation = ServiceLocator.Locate<Animation>();
        animation.Enqueue(moveSequence);
        animation.Enqueue(blockSequence);
        animation.Enqueue(nudgeSequence);
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

    public record struct CellPosition(Point Local, Point Global);
}
