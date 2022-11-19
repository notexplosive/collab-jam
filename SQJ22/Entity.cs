using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SQJ22;

public readonly record struct Entity(EntityData Data, Point Position)
{
    public IEnumerable<CellPosition> CellPositions()
    {
        foreach (var localPosition in Data.Body.Cells())
        {
            yield return new CellPosition(localPosition, localPosition + Position);
        }
    }

    public record struct CellPosition(Point Local, Point Global);
}
