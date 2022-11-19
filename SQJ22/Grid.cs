using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SQJ22;

public class Grid
{
    private readonly HashSet<Point> _cells = new();

    public Grid AddCell(int x, int y)
    {
        _cells.Add(new Point(x, y));
        return this;
    }

    public IEnumerable<Point> Cells()
    {
        return _cells;
    }

    public bool Contains(Point localCellPosition)
    {
        return _cells.Contains(localCellPosition);
    }
}
