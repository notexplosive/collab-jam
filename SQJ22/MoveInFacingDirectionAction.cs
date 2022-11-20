using System;
using System.Collections.Generic;
using ExplogineMonoGame;
using ExTween;

namespace SQJ22;

public class MoveInFacingDirectionAction : ITokenAction
{
    public ITween Execute(GridSpace space, EntityData data)
    {
        return new DynamicTween(() =>
        {
            var original = space.GetEntityFromData(data);
            var moved = original with {Position = original.Position + original.Direction.AsPoint};
            space.RemoveEntityMatchingData(original.Data);

            var canMove = true;
            var nudgeSequence = new SequenceTween();
            var moveSequence = new SequenceTween();
            var blockSequence = new SequenceTween();

            HashSet<EntityData> entitiesAlreadyNudged = new();

            foreach (var newCellPosition in moved.CellPositions())
            {
                if (space.HasEntityAt(newCellPosition.Global))
                {
                    canMove = false;
                    var nudgedMaybeEntity = space.GetEntityAt(newCellPosition.Global);
                    if (!nudgedMaybeEntity.HasValue)
                    {
                        throw new Exception("API violation, Space said we had an entity here, but we didn't");
                    }

                    var nudgedEntity = nudgedMaybeEntity.Value;
                    var nudgedEntityData = nudgedEntity.Data;


                    if (!entitiesAlreadyNudged.Contains(nudgedEntityData))
                    {
                        nudgeSequence.Add(GameplayEvents.AnimateNudged(nudgedEntityData.RenderHandle,
                            original.Direction.AsPoint));
                        nudgeSequence.Add(nudgedEntityData.Behavior.Nudged.Execute(space, nudgedEntityData));
                    }

                    entitiesAlreadyNudged.Add(nudgedEntityData);

                }

                if (!space.ContainsCell(newCellPosition.Global))
                {
                    canMove = false;
                }
            }

            if (canMove)
            {
                space.AddEntity(moved);
                moveSequence.Add(GameplayEvents.AnimateMove(moved.Data.RenderHandle, original.Direction.AsPoint));
                moveSequence.Add(original.Data.Behavior.Moved.Execute(space, original.Data));
            }
            else
            {
                space.AddEntity(original);
                blockSequence.Add(GameplayEvents.AnimateBlock(original.Data.RenderHandle, original.Direction.AsPoint));
                blockSequence.Add(original.Data.Behavior.Blocked.Execute(space, original.Data));
            }

            return new SequenceTween()
                    .Add(moveSequence)
                    .Add(blockSequence)
                    .Add(nudgeSequence)
                ;
        });
    }
}

public class RotateAction : ITokenAction
{
    private readonly Rotation _rotation;

    public RotateAction(Rotation rotation)
    {
        _rotation = rotation;
    }

    public ITween Execute(GridSpace space, EntityData data)
    {
        return new CallbackTween(() =>
        {
            var original = space.GetEntityFromData(data);
            space.RemoveEntity(original);
            var rotatedEntity = original with {Direction = original.Direction.Next(_rotation)};
            space.AddEntity(rotatedEntity);
        });
    }
}
