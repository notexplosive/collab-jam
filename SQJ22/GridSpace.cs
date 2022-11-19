using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SQJ22;

public class GridSpace
{
    private readonly List<Entity> _entities = new();

    public GridSpace(int width, int height)
    {
        Size = new Point(width, height);
    }

    public Point Size { get; }

    public EntityData GetEntityDataAt(Point targetPosition)
    {
        var entity = GetEntityAt(targetPosition);

        if (entity.HasValue)
        {
            return entity.Value.Data;
        }

        return null;
    }

    public Entity? GetEntityAt(Point targetPosition)
    {
        foreach (var record in _entities)
        {
            foreach (var cell in record.CellPositions())
            {
                if (cell.Global == targetPosition)
                {
                    return record;
                }
            }
        }

        return null;
    }

    public bool CanAddEntity(EntityData entityData, Point position)
    {
        var newRecord = new Entity(this, entityData, position);
        return CanAddEntity(newRecord);
    }

    public EntityData AddEntityFromData(EntityData entityData, Point position)
    {
        var newRecord = new Entity(this, entityData, position);
        if (CanAddEntity(newRecord))
        {
            AddEntity(newRecord);
            return entityData;
        }

        throw new Exception("Cannot add entity");
    }

    public void AddEntity(Entity entity)
    {
        _entities.Add(entity);
    }

    public bool HasEntityAt(Point cell)
    {
        return GetEntityDataAt(cell) != null;
    }

    private bool HasEntityData(EntityData targetEntityData)
    {
        foreach (var record in _entities)
        {
            if (record.Data == targetEntityData)
            {
                return true;
            }
        }

        return false;
    }

    public bool CanAddEntity(Entity entity)
    {
        if (HasEntityData(entity.Data))
        {
            return false;
        }

        foreach (var cell in entity.CellPositions())
        {
            if (HasEntityAt(cell.Global))
            {
                return false;
            }

            if (!ContainsCell(cell.Global))
            {
                return false;
            }
        }

        return true;
    }

    public IEnumerable<Entity> Entities()
    {
        return _entities;
    }

    public void AttemptMoveEntity(EntityData targetData, Point offset)
    {
        var oldEntity = GetEntityFromData(targetData);

        var newEntity = new Entity(this, targetData, oldEntity.Position + offset);
        RemoveEntity(oldEntity);
        if (CanAddEntity(newEntity))
        {
            AddEntity(newEntity);
        }
        else
        {
            AddEntity(oldEntity);
        }
    }

    public void RemoveEntity(Entity entity)
    {
        _entities.Remove(entity);
    }

    private Entity GetEntityFromData(EntityData targetData)
    {
        foreach (var record in _entities)
        {
            if (record.Data == targetData)
            {
                return record;
            }
        }

        throw new Exception("No entity found from data");
    }

    public bool ContainsCell(Point cellPosition)
    {
        return cellPosition.X >= 0 && cellPosition.Y >= 0 && cellPosition.X < Size.X && cellPosition.Y < Size.Y;
    }
}
