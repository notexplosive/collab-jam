using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SQJ22;

public readonly record struct Entity(EntityData Data, Point Position)
{
    public IEnumerable<Point> CellPositions()
    {
        foreach (var localCell in Data.Body.Cells())
        {
            yield return localCell + Position;
        }
    }
}
