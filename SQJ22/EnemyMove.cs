﻿using System;
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
        public void Execute();
        public string Description();

        public void DrawPreview(Painter painter, RenderSettings settings, Depth depth);
    }

    public class AttackZone : IAttack
    {
        private readonly Action<GridSpace, Grid> _execute;
        private readonly GridSpace _space;
        private readonly Grid _zone;

        public AttackZone(GridSpace space, Grid zone, Action<GridSpace, Grid> execute)
        {
            _space = space;
            _zone = zone;
            _execute = execute;
        }

        public void Execute()
        {
            _execute(_space, _zone);
        }

        public string Description()
        {
            // Need to split this up more, execute + zone should be a type "ZoneAttack"
            return "Attack the board";
        }

        public void DrawPreview(Painter painter, RenderSettings settings, Depth depth)
        {
            foreach (var cell in _zone.Cells())
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

        public void Execute()
        {
            _execute(_space, _data);
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

        public void Execute()
        {
            ServiceLocator.Locate<Battle>().CurrentEncounter.PlayerAgent.TakeDamage(_damage);
        }

        public string Description()
        {
            return $"Deal {_damage} damage";
        }

        public void DrawPreview(Painter painter, RenderSettings settings, Depth depth)
        {
        }
    }
}
