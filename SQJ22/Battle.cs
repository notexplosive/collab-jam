namespace SQJ22;

public class Battle
{
    public Battle()
    {
        InternalEncounter = new BattleEncounter();
    }

    public IEncounter InternalEncounter { get; private set; }
    public BattleEncounter BattleEncounter => InternalEncounter as BattleEncounter;

    public void StartNextBattle()
    {
        InternalEncounter = new BattleEncounter();
    }
    
    public void StartShop()
    {
        InternalEncounter = new ShopEncounter();
    }
}