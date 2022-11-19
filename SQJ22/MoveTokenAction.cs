using ExTween;
using Microsoft.Xna.Framework;

namespace SQJ22;

public class MoveTokenAction : ITokenAction
{
    public ITween Execute(Entity entity)
    {
        return new CallbackTween(()=>entity.AttemptMove(new Point(1, 0)));
    }
}
