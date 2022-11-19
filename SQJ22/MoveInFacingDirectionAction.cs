using System;
using ExTween;

namespace SQJ22;

public class MoveInFacingDirectionAction : ITokenAction
{
    public ITween Execute(GridSpace space, EntityData data)
    {
        return new DynamicTween(() =>
        {
            var original = space.GetEntityFromData(data);
            var newEntity = original with {Position = original.Position + original.Direction.AsPoint};
            space.RemoveEntityMatchingData(original.Data);

            var canMove = true;
            var nudgeSequence = new SequenceTween();
            var moveSequence = new SequenceTween();
            var blockSequence = new SequenceTween();

            foreach (var newCellPosition in newEntity.CellPositions())
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
                    nudgeSequence.Add(nudgedEntityData.Behavior.Nudged.Execute(space, nudgedEntityData));
                    nudgeSequence.Add(GameplayEvents.AnimateNudged(nudgedEntityData.RenderHandle,
                        original.Direction.AsPoint));
                }

                if (!space.ContainsCell(newCellPosition.Global))
                {
                    canMove = false;
                }
            }

            if (canMove)
            {
                space.AddEntity(newEntity);
                moveSequence.Add(original.Data.Behavior.Moved.Execute(space, original.Data));
                moveSequence.Add(GameplayEvents.AnimateMove(newEntity.Data.RenderHandle, original.Direction.AsPoint));
            }
            else
            {
                space.AddEntity(original);
                blockSequence.Add(original.Data.Behavior.Blocked.Execute(space, original.Data));
                blockSequence.Add(GameplayEvents.AnimateBlock(original.Data.RenderHandle, original.Direction.AsPoint));
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
