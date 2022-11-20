using ExplogineMonoGame;
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

    public static ITween TriggerTap(Entity entity)
    {
        return new SequenceTween()
                .Add(GameplayEvents.AnimateTap(entity.Data.RenderHandle))
                .Add(GameplayEvents.TriggerTapAdjacent(entity))
                .Add(entity.Data.Behavior.Tapped.Execute(entity.Space, entity.Data))
            ;
    }

    private static ITween AnimateTap(RenderHandle tappedEntityRenderHandle)
    {
        var randomVec = ()=> new Vector2(Client.Random.Dirty.NextFloat() - 0.5f, Client.Random.Dirty.NextFloat() - 0.5f) * 0.25f;
        return new SequenceTween()
                .Add(new Tween<Vector2>(tappedEntityRenderHandle.Offset, randomVec(), 0.02f, Ease.QuadFastSlow))
                .Add(new Tween<Vector2>(tappedEntityRenderHandle.Offset, randomVec(), 0.02f, Ease.QuadFastSlow))
                .Add(new Tween<Vector2>(tappedEntityRenderHandle.Offset, randomVec(), 0.02f, Ease.QuadFastSlow))
                .Add(new Tween<Vector2>(tappedEntityRenderHandle.Offset, Vector2.Zero, 0.02f, Ease.QuadFastSlow))
            ;
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
        return new SequenceTween()
            ;
    }

    public static ITween AnimateEnemyTurn()
    {
        var encounter = ServiceLocator.Locate<Battle>().BattleEncounter;
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
            ;
    }

    public static ITween AnimatePlayerAttack(BattleAgent enemy, int damage)
    {
        return new SequenceTween()
                .Add(new CallbackTween(() => enemy.TakeDamage(damage)))
            ;
    }

    public static ITween IncrementStatusEffectTurn()
    {
        return new SequenceTween()
            .Add(new CallbackTween(() =>
            {
                ServiceLocator.Locate<Battle>().BattleEncounter.StatusEffects.IncrementTurn();
            }));
    }
}
