namespace RotarySpoon.API.Contracts
{
    public class UpdateTodoListRequest
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}