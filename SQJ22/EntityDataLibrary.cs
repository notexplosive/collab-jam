namespace SQJ22;

public static class EntityDataLibrary
{
    public static readonly EntityData MeleeAttack =
        new(
            new EntityName("Melee Attack"),
            new Grid()
                .AddCell(0, 0)
                .AddCell(0, 1)
                .AddCell(0, 2)
                .AddCell(1, 0)
                .AddCell(1, 1)
                .AddCell(1, 2)
            ,
            new TokenBehavior()
                .OnTapped(new DealDamageAction(5))
            , 
            new DebugRenderer()
        );
    
    public static readonly EntityData Glider =
        new(
            new EntityName("Glide Attack"),
            new Grid()
                .AddCell(0, 0)
                .AddCell(0, 1)
                .AddCell(0, 2)
                .AddCell(1, 0)
                .AddCell(1, 2)
            ,
            new TokenBehavior()
                .OnTapped(new MoveInFacingDirectionAction())
                .OnMoved(
                    new DealDamageAction(3),
                    new MoveInFacingDirectionAction()
                    )
                .OnBlocked(new ReverseDirectionAction())
            , 
            new DebugRenderer()
        );
}
