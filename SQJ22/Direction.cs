using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SQJ22;

public record Direction
{
    public static readonly Direction Down = new(new Point(0, 1), "Down");
    public static readonly Direction Up = new(new Point(0, -1), "Up");
    public static readonly Direction Left = new(new Point(-1, 0), "Left");
    public static readonly Direction Right = new(new Point(1, 0), "Right");
    public static readonly Direction None = new(new Point(0, 0), "None");

    private Direction(Point asPoint, string name)
    {
        AsPoint = asPoint;
        Name = name;
    }

    public string Name { get; }

    public Point AsPoint { get; }

    public Point ToPoint()
    {
        return AsPoint;
    }

    public Vector2 ToVector2()
    {
        return AsPoint.ToVector2();
    }

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

    public float ToAngle()
    {
        if (this == Direction.Up)
        {
            return -MathF.PI / 2;
        }

        if (this == Direction.Right)
        {
            return 0;
        }

        if (this == Direction.Down)
        {
            return MathF.PI / 2;
        }

        if (this == Direction.Left)
        {
            return MathF.PI;
        }

        return 0;
    }

    public static IEnumerable<Direction> Each(bool includeNone = true)
    {
        yield return Direction.Right;
        yield return Direction.Up;
        yield return Direction.Left;
        yield return Direction.Down;
        if (includeNone)
        {
            yield return Direction.None;
        }
    }

    public Direction Opposite()
    {
        if (this == Direction.Up)
        {
            return Direction.Down;
        }

        if (this == Direction.Right)
        {
            return Direction.Left;
        }

        if (this == Direction.Down)
        {
            return Direction.Up;
        }

        if (this == Direction.Left)
        {
            return Direction.Right;
        }

        return Direction.None;
    }
}
