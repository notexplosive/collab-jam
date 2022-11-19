using Microsoft.Xna.Framework;

namespace SQJ22;

public readonly record struct RenderSettings(Vector2 Position, int CellSize)
{
    public Vector2 CellPositionToRenderedPosition(Point cellPosition)
    {
        return cellPosition.ToVector2() * CellSize + Position;
    }

    public Point CellSizeAsPoint => new(CellSize);

    public Point GetGridPositionFromWorldPosition(Vector2 worldPosition)
    {
        return ((worldPosition - Position) / CellSize).ToPoint();
    }
}
