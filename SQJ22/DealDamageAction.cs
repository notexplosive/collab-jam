using System;
using ExplogineMonoGame;
using ExTween;

namespace SQJ22;

public class DealDamageAction : ITokenAction
{
    private readonly int _damageAmount;

    public DealDamageAction(int i)
    {
        _damageAmount = i;
    }

    public ITween Execute(GridSpace space, EntityData data)
    {
        return new SequenceTween()
            .Add(new CallbackTween(() =>
            {
                var battle = ServiceLocator.Locate<Battle>();
                battle.BattleEncounter.PlayerMove.AddPendingDamage(_damageAmount);
            }));
    }
}