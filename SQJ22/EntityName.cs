namespace SQJ22;

public record EntityName(string Text)
{
    public static EntityName Empty = new("None");
}
