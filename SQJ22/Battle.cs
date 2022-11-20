namespace SQJ22;

public class Battle
{
    public Battle()
    {
        InternalEncounter = new BattleEncounter();
    }

    public IEncounter InternalEncounter { get; private set; }
    public BattleEncounter BattleEncounter => InternalEncounter as BattleEncounter;

    public void StartNextEncounter()
    {
        InternalEncounter = new BattleEncounter();
    }
}
