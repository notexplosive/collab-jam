﻿using ExplogineMonoGame;
using ExTween;
using Microsoft.Xna.Framework;

namespace SQJ22;

public static class GameplayEvents
{
    private static ITween TriggerTapAdjacent(Entity sourceEntity)
    {
        return new SequenceTween()
                .Add(new DynamicTween(() =>
                {
                    var result = new SequenceTween();
                    foreach (var entity in sourceEntity.GetAdjacentEntities())
                    {
                        result.Add(entity.Data.Behavior.AdjacentTapped.Execute(entity.Space, entity.Data));
                        result.Add(GameplayEvents.AnimateTapAdjacent());
                    }

                    return result;
                }))
            ;
    }

    private static CallbackTween LogMessage(string message)
    {
        return new CallbackTween(() => { Client.Debug.Log(message); });
    }

    public static ITween TriggerTap(Entity entity)
    {
        return new SequenceTween()
                .Add(GameplayEvents.TriggerTapAdjacent(entity))
                .Add(entity.Data.Behavior.Tapped.Execute(entity.Space, entity.Data))
                .Add(GameplayEvents.AnimateTap(entity.Data.RenderHandle))
            ;
    }

    private static ITween AnimateTap(RenderHandle tappedEntityRenderHandle)
    {
        // todo
        return new SequenceTween();
    }

    public static ITween AnimateNudged(RenderHandle nudgedEntityHandle, Point offset)
    {
        return new SequenceTween()
                .Add(new Tween<Vector2>(nudgedEntityHandle.Offset, offset.ToVector2() / 4, 0.05f, Ease.QuadFastSlow))
                .Add(new Tween<Vector2>(nudgedEntityHandle.Offset, Vector2.Zero, 0.05f, Ease.QuadSlowFast))
            ;
    }

    public static ITween AnimateMove(RenderHandle movedEntityRenderHandle, Point offset)
    {
        return new SequenceTween()
                .Add(new CallbackTween(() => movedEntityRenderHandle.Offset.Value = -offset.ToVector2()))
                .Add(new Tween<Vector2>(movedEntityRenderHandle.Offset, Vector2.Zero, 0.10f, Ease.QuadFastSlow))
            ;
    }

    public static ITween AnimateBlock(RenderHandle blockedEntityRenderHandle, Point offset)
    {
        return new SequenceTween()
                .Add(new Tween<Vector2>(blockedEntityRenderHandle.Offset, offset.ToVector2() / 4, 0.05f,
                    Ease.QuadFastSlow))
                .Add(new Tween<Vector2>(blockedEntityRenderHandle.Offset, Vector2.Zero, 0.10f, Ease.QuadSlowFast))
            ;
    }

    private static ITween AnimateTapAdjacent()
    {
        // todo
        return new SequenceTween();
    }

    public static ITween AnimateEnemyTurn()
    {
        var encounter = ServiceLocator.Locate<Battle>().CurrentEncounter;
        return new SequenceTween()
                .Add(new DynamicTween(() =>
                {
                    var result = new SequenceTween();
                    var shouldGetNewMove = encounter.EnemyMove.CurrentAttack.Execute();

                    if (shouldGetNewMove)
                    {
                        result.Add(new CallbackTween(encounter.ClearMove));
                    }
                    
                    result.Add(new WaitSecondsTween(0.5f));
                    if (shouldGetNewMove)
                    {
                        result.Add(new CallbackTween(encounter.PlanNewEnemyMove));
                    }

                    return result;
                }))
                .Add(GameplayEvents.LogMessage("Enemy finished attacking."))
            ;
    }

    public static ITween AnimatePlayerAttack(BattleAgent enemy, int damage)
    {
        return new SequenceTween()
            .Add(new CallbackTween(() => enemy.TakeDamage(damage)))
            .Add(GameplayEvents.LogMessage($"Dealt {damage} damage"));
    }

    public static ITween IncrementStatusEffectTurn()
    {
        return new SequenceTween()
            .Add(new CallbackTween(() =>
            {
                ServiceLocator.Locate<Battle>().CurrentEncounter.StatusEffects.IncrementTurn();
            }));
    }
}
