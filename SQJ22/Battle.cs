namespace SQJ22;

public class Battle
{
    public Battle()
    {
        CurrentEncounter = new Encounter();
    }

    public Encounter CurrentEncounter { get; private set; }

    public void StartNextEncounter()
    {
        CurrentEncounter = new Encounter();
    }
}
