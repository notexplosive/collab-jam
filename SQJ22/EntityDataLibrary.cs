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

    public static readonly EntityData Crystal =
        new(
            new EntityName("Crystal"),
            new Grid()
                .AddCell(0, 0)
                .AddCell(0, 1)
                .AddCell(0, 2)
            ,
            new TokenBehavior()
                // todo
                .OnTapped(new DealDamageAction(5))
            ,
            new SpriteRenderer("img_crystal")
        );

    public static EntityData Rune =
        new(
            new EntityName("Rune"),
            new Grid()
                .AddCell(0, 0)
                .AddCell(1, 0)
                .AddCell(2, 0)
            ,
            new TokenBehavior()
                .OnTapped(
                    // todo
                    new DealDamageAction(5)
                )
            ,
            new SpriteRenderer("img_rune")
        );

    public static EntityData E1 =
        new(
            new EntityName("E1"),
            new Grid()
                .AddCell(1, 1)
                .AddCell(1, 0)
                .AddCell(1, 2)
                .AddCell(0, 1)
                .AddCell(2, 1)
            ,
            new TokenBehavior()
                .OnTapped(
                    new MoveInFacingDirectionAction(),
                    new RotateAction(Rotation.CounterClockwise)
                )
            ,
            new SpriteRenderer("img_doohickey"));
}
