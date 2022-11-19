using Microsoft.Xna.Framework;

namespace SQJ22;

public class MoveTokenAction : ITokenAction
{
    public void Execute(Entity entity)
    {
        entity.AttemptMove(new Point(1, 0));
    }
}
