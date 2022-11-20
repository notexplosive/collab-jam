using ExTween;

namespace SQJ22;

public class ReverseDirectionAction : ITokenAction
{
    public ITween Execute(GridSpace space, EntityData data)
    {
        return new CallbackTween(() =>
        {
            var entity = space.GetEntityFromData(data);
            space.RemoveEntity(entity);
            var flippedEntity = entity with {Direction = entity.Direction.Opposite()};
            space.AddEntity(flippedEntity);
        });
    }
}
