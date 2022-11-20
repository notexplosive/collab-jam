using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SQJ22;

public readonly record struct ZoneAttack(Grid Zone, Action<Grid, GridSpace, Battle> OnExecute)
{
    public void Execute()
    {
        OnExecute(Zone, ServiceLocator.Locate<GridSpace>(), ServiceLocator.Locate<Battle>());
    }

    public IEnumerable<Point> Cells()
    {
        return Zone.Cells();
    }
}
