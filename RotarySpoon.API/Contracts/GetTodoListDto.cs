namespace RotarySpoon.API.Contracts;

public sealed class GetTodoListDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}