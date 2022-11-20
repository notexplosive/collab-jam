using ExTween;

namespace SQJ22;

public class Battle
{
    public PlayerMove CurrentPlayerMove { get; private set; }

    public Battle()
    {
        ResetPlayerMove();
    }

    private void ResetPlayerMove()
    {
        CurrentPlayerMove = new PlayerMove(5);
    }

    public void ExecuteEnemyTurn()
    {
        ServiceLocator.Locate<Animation>().Enqueue(GameplayEvents.AnimateEnemyTurn());
        
        ResetPlayerMove();
    }
}
