namespace Api.Domain;

public class Income : IEntityBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Source { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}