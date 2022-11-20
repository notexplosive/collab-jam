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
        var newRecord = new Entity(this, entityData, position, Direction.None);
        return CanAddEntity(newRecord);
    }

    public EntityData AddEntityFromData(EntityData entityData, Point position, Direction direction)
    {
        var newRecord = new Entity(this, entityData, position, direction);
        if (CanAddEntity(newRecord))
        {
            AddEntity(newRecord);
            return entityData;
        }

        throw new Exception("Cannot add entity");
    }

    public void AddEntity(Entity newEntity)
    {
        foreach (var entity in _entities)
        {
            if (entity.Data == newEntity.Data)
            {
                throw new Exception("Duplicate entity");
            }
        }
        
        _entities.Add(newEntity);
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

    public void AttemptWarpEntity(Entity entityBeforeWarp, Point offset)
    {
        var newEntity = entityBeforeWarp with {Position = entityBeforeWarp.Position + offset};
        RemoveEntity(entityBeforeWarp);
        if (CanAddEntity(newEntity))
        {
            AddEntity(newEntity);
        }
        else
        {
            AddEntity(entityBeforeWarp);
        }
    }

    public void RemoveEntity(Entity entity)
    {
        _entities.Remove(entity);
    }

    public void RemoveEntityMatchingData(EntityData data)
    {
        RemoveEntity(GetEntityFromData(data));
    }

    public Entity GetEntityFromData(EntityData targetData)
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

    public IEnumerable<EntityData> GetEntityDatasInZone(Grid zone)
    {
        var set = new HashSet<EntityData>();
        foreach (var cell in zone.Cells())
        {
             set.Add(GetEntityDataAt(cell));
        }

        return set;
    }
}
