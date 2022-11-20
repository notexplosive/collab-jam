namespace SQJ22;

public class PlayerMove
{
    private readonly int _maxEnergy;

    public PlayerMove(int maxEnergy)
    {
        _maxEnergy = maxEnergy;
        Energy = _maxEnergy;
    }

    public int Energy { get; private set; }

    public void LoseEnergy()
    {
        Energy--;
    }

    public bool IsOutOfEnergy()
    {
        return Energy <= 0;
    }

    public void RestoreEnergy(int amount = 1)
    {
        Energy += amount;
        if (Energy > _maxEnergy)
        {
            Energy = _maxEnergy;
        }
    }
}