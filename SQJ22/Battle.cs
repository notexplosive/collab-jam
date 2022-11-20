using ExTween;

namespace SQJ22;

public class Battle
{
    private readonly BattleAgent _enemy;
    private readonly BattleAgent _player;

    public Battle()
    {
        _enemy = new BattleAgent(100);
        _player = new BattleAgent(50);
        CurrentPlayerMove = new PlayerMove(5);
    }

    public PlayerMove CurrentPlayerMove { get; }

    public void ExecutePlayerAndEnemyTurn()
    {
        var animation = ServiceLocator.Locate<Animation>();
        
        animation.Enqueue(new DynamicTween(() => new SequenceTween()
            .Add(GameplayEvents.AnimatePlayerAttack(_enemy, CurrentPlayerMove.PendingDamage))
            .Add(GameplayEvents.AnimateEnemyTurn())
            .Add(new CallbackTween(CurrentPlayerMove.StartTurn))));
    }
}
