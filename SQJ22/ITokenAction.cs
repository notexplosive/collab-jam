using ExTween;

namespace SQJ22;

public interface ITokenAction
{
    public ITween Execute(GridSpace space, EntityData data);
}