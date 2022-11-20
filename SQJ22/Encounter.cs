﻿using ExTween;

namespace SQJ22;

public class Encounter
{
    public BattleAgent EnemyAgent { get; }

    public Encounter()
    {
        EnemyAgent = new BattleAgent(100);
        PlayerAgent = new BattleAgent(50);
        PlayerMove = new PlayerMove(5);
        EnemyMove = new EnemyMove();
    }

    public BattleAgent PlayerAgent { get; }
    public StatusEffects StatusEffects { get; } = new();
    public PlayerMove PlayerMove { get; }
    public EnemyMove EnemyMove { get; }

    public void ExecutePlayerAndEnemyTurn()
    {
        var animation = ServiceLocator.Locate<Animation>();

        animation.Enqueue(new DynamicTween(() => new SequenceTween()
            .Add(GameplayEvents.IncrementStatusEffectTurn())
            .Add(GameplayEvents.AnimatePlayerAttack(EnemyAgent, PlayerMove.PendingDamage))
            .Add(GameplayEvents.AnimateEnemyTurn())
            .Add(new CallbackTween(PlayerMove.StartTurn))));
    }

    public void PlanNewEnemyMove()
    {
        EnemyMove.CurrentAttack = AttackPool.Next();
    }

    public void ClearMove()
    {
        EnemyMove.CurrentAttack = AttackPool.EmptyAttack();
    }
}
