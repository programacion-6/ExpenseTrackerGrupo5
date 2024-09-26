namespace Api.Domain;

public class Income : IEntityBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
    public string Source { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; }
}