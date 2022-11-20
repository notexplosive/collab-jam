using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SQJ22;

public readonly record struct ZoneAttack(Grid Zone, Point Offset, Action<Grid, GridSpace, Battle, Point> OnExecute)
{
    public void Execute()
    {
        var space = ServiceLocator.Locate<GridSpace>();
        OnExecute(Zone, space, ServiceLocator.Locate<Battle>(), Offset);
    }

    public IEnumerable<Point> Cells()
    {
        foreach (var cell in Zone.Cells())
        {
            yield return cell + Offset;
        }
    }
}
