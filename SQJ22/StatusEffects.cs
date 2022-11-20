using System;
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
        return new Instance(new ZoneTarget(statusEffectZone), Templates.Lockout, duration);
    }

    public static Instance CreateLockoutEntity(GridSpace space, EntityData target, int duration = 1)
    {
        return new Instance(new EntityTarget(target), Templates.Lockout, duration);
    }

    public class Instance
    {
        public Instance(ITarget target, Template template, int numberOfTurns)
        {
            Target = target;
            NumberOfTurns = numberOfTurns;
            Template = template;
        }

        public ITarget Target { get; }
        public int NumberOfTurns { get; private set; }
        public Template Template { get; }

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

        public EntityTarget(EntityData targetData)
        {
            _space = ServiceLocator.Locate<GridSpace>();
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

    public class Template
    {
        private readonly Lazy<Texture2D> _lazyTexture;

        public Template(string textureName)
        {
            _lazyTexture = new Lazy<Texture2D>(() => Client.Assets.GetTexture(textureName));
        }

        public Texture2D Texture => _lazyTexture.Value;

        public Instance CreateInstance(Grid zone)
        {
            return new Instance(new ZoneTarget(zone), this, 1);
        }
        
        public Instance CreateInstance(EntityData data)
        {
            return new Instance(new EntityTarget(data), this, 1);
        }
    }

    public static class Templates
    {
        public static readonly Template Exclamation = new("exclamation");
        public static readonly Template Lockout = new("lockout");
    }
}
