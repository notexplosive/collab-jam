using ExTween;

namespace SQJ22;

public interface ITokenAction
{
    public ITween Execute(Entity entity);
}