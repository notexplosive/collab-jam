using Microsoft.Xna.Framework;

namespace SQJ22;

public record Direction
{
    public static readonly Direction Down = new(new Point(0, 1));
    public static readonly Direction Up = new(new Point(0, -1));
    public static readonly Direction Left = new(new Point(-1, 0));
    public static readonly Direction Right = new(new Point(1, 0));
    public static readonly Direction None = new(new Point(0, 0));

    private Direction(Point asPoint)
    {
        AsPoint = asPoint;
    }

    public Point AsPoint { get; }

    public Direction NextCounterClockwise()
    {
        if (this == Direction.Up)
        {
            return Direction.Left;
        }

        if (this == Direction.Right)
        {
            return Direction.Up;
        }

        if (this == Direction.Down)
        {
            return Direction.Right;
        }

        if (this == Direction.Left)
        {
            return Direction.Down;
        }

        return Direction.None;
    }

    public Direction NextClockwise()
    {
        if (this == Direction.Up)
        {
            return Direction.Right;
        }

        if (this == Direction.Right)
        {
            return Direction.Down;
        }

        if (this == Direction.Down)
        {
            return Direction.Left;
        }

        if (this == Direction.Left)
        {
            return Direction.Up;
        }

        return Direction.None;
    }

    public Direction Next(Rotation rotation)
    {
        return rotation switch
        {
            Rotation.Clockwise => NextClockwise(),
            Rotation.CounterClockwise => NextCounterClockwise(),
            _ => Direction.None
        };
    }
}
