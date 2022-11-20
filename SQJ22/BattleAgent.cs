using System;

namespace SQJ22;

public class BattleAgent
{
    public event Action TookDamage;
    public event Action Died;
    
    private readonly int _maxHealth;

    public BattleAgent(int maxHealth)
    {
        _maxHealth = maxHealth;
        Health = _maxHealth;
    }

    public int Health { get; private set; }

    public void TakeDamage(int amount = 1)
    {
        Health -= amount;
        TookDamage?.Invoke();

        if (Health < 0)
        {
            Died?.Invoke();
        }
    }
}
