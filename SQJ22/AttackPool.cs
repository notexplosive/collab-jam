using ExplogineMonoGame;

namespace SQJ22;

public static class AttackPool
{
    public static EnemyMove.IAttack Next()
    {
        return Client.Random.Clean.GetRandomElement(AttackPool.AllAttacks());
    }

    public static EnemyMove.IAttack[] AllAttacks()
    {
        var lock3X3 = new ZoneAttack(
            new Grid()
                .AddCell(0, 0)
                .AddCell(1, 0)
                .AddCell(2, 0)
                .AddCell(0, 1)
                .AddCell(1, 1)
                .AddCell(2, 1)
                .AddCell(0, 2)
                .AddCell(1, 2)
                .AddCell(2, 2)
            ,
            ServiceLocator.Locate<GridSpace>().GetRandomCell(),
            (zone, space, battle, offset) =>
            {
                foreach (var data in space.GetEntityDatasInZone(zone, offset))
                {
                    battle.BattleEncounter.StatusEffects.AddStatusEffect(
                        StatusEffects.Templates.Lockout.CreateInstance(data));
                }
            });

        return new EnemyMove.IAttack[]
        {
            new EnemyMove.AttackZone(lock3X3),
            new EnemyMove.DealDamage(10),
            new EnemyMove.ChargeAttack(new EnemyMove.DealDamage(25), 1)
        };
    }

    public static EnemyMove.IAttack EmptyAttack()
    {
        return new EmptyAttack();
    }
}
