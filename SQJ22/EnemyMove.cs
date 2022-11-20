using System;
using ExplogineCore.Data;
using ExplogineMonoGame;
using Microsoft.Xna.Framework;

namespace SQJ22;

public class EnemyMove
{
    public IAttack CurrentAttack { get; set; }

    public EnemyMove()
    {
        CurrentAttack = new DealDamage(5);
    }
    
    public interface IAttack
    {
        public bool Execute();
        public string Description();

        public void DrawPreview(Painter painter, RenderSettings settings, Depth depth);
    }

    public class AttackZone : IAttack
    {
        private readonly ZoneAttack _zoneAttack;

        public AttackZone(ZoneAttack zoneAttack)
        {
            _zoneAttack = zoneAttack;
        }

        public bool Execute()
        {
            _zoneAttack.Execute();
            return true;
        }

        public string Description()
        {
            // Need to split this up more, execute + zone should be a type "ZoneAttack"
            return "Attack the board";
        }

        public void DrawPreview(Painter painter, RenderSettings settings, Depth depth)
        {
            foreach (var cell in _zoneAttack.Cells())
            {
                DrawMacros.DrawOverlayTextureOnCell(painter, cell, settings, Client.Assets.GetTexture("exclamation"),
                    Vector2.Zero, depth);
            }
        }
    }

    public class AttackToken : IAttack
    {
        private readonly EntityData _data;
        private readonly Action<GridSpace, EntityData> _execute;
        private readonly GridSpace _space;

        public AttackToken(GridSpace space, EntityData data, Action<GridSpace, EntityData> execute)
        {
            _space = space;
            _data = data;
            _execute = execute;
        }

        public Entity Entity => _space.GetEntityFromData(_data);

        public bool Execute()
        {
            _execute(_space, _data);
            return true;
        }

        public string Description()
        {
            return "Attack a token";
        }

        public void DrawPreview(Painter painter, RenderSettings settings, Depth depth)
        {
            DrawMacros.DrawOverlayTextureOnEntity(painter, Entity, settings, Client.Assets.GetTexture("exclamation"),
                Vector2.Zero, depth);
        }
    }

    public class DealDamage : IAttack
    {
        private readonly int _damage;

        public DealDamage(int damage)
        {
            _damage = damage;
        }

        public bool Execute()
        {
            ServiceLocator.Locate<Battle>().CurrentEncounter.PlayerAgent.TakeDamage(_damage);
            return true;
        }

        public string Description()
        {
            return $"Deal {_damage} damage";
        }

        public void DrawPreview(Painter painter, RenderSettings settings, Depth depth)
        {
        }
    }

    public class ChargeAttack : IAttack
    {
        private readonly IAttack _chargedAttack;
        private int _numberOfTurnsLeft;

        public ChargeAttack(IAttack chargedAttack, int duration)
        {
            _chargedAttack = chargedAttack;
            _numberOfTurnsLeft = duration;
        }

        public bool Execute()
        {
            if (_numberOfTurnsLeft > 0)
            {
                _numberOfTurnsLeft--;
                return false;
            }
            else
            {
                _chargedAttack.Execute();
                return true;
            }
        }

        public string Description()
        {
            return (_numberOfTurnsLeft > 0 ? $"In {_numberOfTurnsLeft} turns: " : "") + _chargedAttack.Description();
        }

        public void DrawPreview(Painter painter, RenderSettings settings, Depth depth)
        {
            _chargedAttack.DrawPreview(painter, settings, depth);
        }
    }
}