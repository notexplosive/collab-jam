using ExTween;

namespace SQJ22;

public class MoveInFacingDirectionAction : ITokenAction
{
    public ITween Execute(GridSpace space, EntityData data)
    {
        return new CallbackTween(() =>
        {
            var original = space.GetEntityFromData(data);
            original.AttemptMove(original.Direction.AsPoint);
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
