using ExTween;

namespace SQJ22;

public class Battle
{
    private readonly BattleAgent _enemy;

    public Battle()
    {
        _enemy = new BattleAgent(100);
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
            .Add(GameplayEvents.AnimatePlayerAttack(_enemy, PlayerMove.PendingDamage))
            .Add(GameplayEvents.AnimateEnemyTurn())
            .Add(new CallbackTween(PlayerMove.StartTurn))));
    }

    public void PlanNewEnemyMove()
    {
        EnemyMove.CurrentAttack = new EnemyMove.AttackZone(ServiceLocator.Locate<GridSpace>(),
            new Grid().AddCell(0, 0).AddCell(1, 1).AddCell(2, 2),
            (space, zone) => { });
    }
}
