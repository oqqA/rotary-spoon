namespace RotarySpoon.DataAccess;
public record TodoItem
{
    public Guid Id { get; init; }
    public string Text { get; init; }
    public DateTimeOffset CreatedDate { get; init; }
}
