using System.Collections.Generic;
using ExplogineMonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SQJ22;

public class StatusEffects
{
    private readonly List<Instance> _instances = new();

    public void AddStatusEffect(Instance instance)
    {
        _instances.Add(instance);
    }

    public void IncrementTurn()
    {
        for (var i = _instances.Count - 1; i >= 0; i--)
        {
            var instance = _instances[i];
            instance.IncrementTurn();
            if (instance.IsExpired())
            {
                _instances.RemoveAt(i);
            }
        }
    }

    public IEnumerable<Instance> Instances()
    {
        return _instances;
    }

    public static Instance CreateLockoutZone(Grid statusEffectZone, int duration = 1)
    {
        return new Instance(new ZoneTarget(statusEffectZone), Client.Assets.GetTexture("lockout"), duration);
    }
    
    public static Instance CreateExclamationZone(Grid statusEffectZone, int duration = 1)
    {
        return new Instance(new ZoneTarget(statusEffectZone), Client.Assets.GetTexture("exclamation"), duration);
    }
    
    public static Instance CreateLockoutEntity(GridSpace space, EntityData target, int duration = 1)
    {
        return new Instance(new EntityTarget(space, target), Client.Assets.GetTexture("exclamation"), duration);
    }
    
    public static Instance CreateExclamationEntity(GridSpace space, EntityData target, int duration = 1)
    {
        return new Instance(new EntityTarget(space, target), Client.Assets.GetTexture("lockout"), duration);
    }

    public class Instance
    {
        public Instance(ITarget target, Texture2D texture, int numberOfTurns)
        {
            Target = target;
            NumberOfTurns = numberOfTurns;
            Texture = texture;
        }

        public ITarget Target { get; }
        public int NumberOfTurns { get; private set; }
        public Texture2D Texture { get; }

        public void IncrementTurn()
        {
            NumberOfTurns--;
        }

        public bool IsExpired()
        {
            return NumberOfTurns <= 0;
        }
    }

    public interface ITarget
    {
        IEnumerable<Point> CellPositions();
    }

    public class EntityTarget : ITarget
    {
        private readonly GridSpace _space;
        private readonly EntityData _targetData;

        public EntityTarget(GridSpace space, EntityData targetData)
        {
            _space = space;
            _targetData = targetData;
        }

        private Entity Entity => _space.GetEntityFromData(_targetData);

        public IEnumerable<Point> CellPositions()
        {
            foreach (var cell in Entity.CellPositions())
            {
                yield return cell.Global;
            }
        }
    }

    public class ZoneTarget : ITarget
    {
        private readonly Grid _grid;

        public ZoneTarget(Grid grid)
        {
            _grid = grid;
        }

        public IEnumerable<Point> CellPositions()
        {
            return _grid.Cells();
        }
    }
}
