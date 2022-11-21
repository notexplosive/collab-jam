using ExTween;

namespace SQJ22;

public class GainEnergyAction : ITokenAction
{
    public ITween Execute(GridSpace space, EntityData data)
    {
        return new SequenceTween()
            .Add(new CallbackTween(() =>
            {
                var battle = ServiceLocator.Locate<Battle>();
                battle.BattleEncounter.PlayerMove.GainEnergy();
            }));
    }
}
