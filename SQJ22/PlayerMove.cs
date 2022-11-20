namespace SQJ22;

public class PlayerMove
{
    private readonly int _maxEnergy;

    public PlayerMove(int maxEnergy)
    {
        _maxEnergy = maxEnergy;
        StartTurn();
    }

    public int Energy { get; private set; }
    public int PendingDamage { get; private set; }

    public void StartTurn()
    {
        PendingDamage = 0;
        Energy = _maxEnergy;
    }

    public void AddPendingDamage(int amount = 0)
    {
        PendingDamage += amount;
    }

    public void LoseOneEnergy()
    {
        Energy--;
    }

    public bool IsOutOfEnergy()
    {
        return Energy <= 0;
    }

    public void GainEnergy(int amount = 1)
    {
        Energy += amount;
        if (Energy > _maxEnergy)
        {
            Energy = _maxEnergy;
        }
    }
}
