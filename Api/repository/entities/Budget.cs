public class Budget : IEntityBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public DateTime Month { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal CurrentAmount { get; set; }
}