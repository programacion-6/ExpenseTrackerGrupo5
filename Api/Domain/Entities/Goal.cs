namespace Api.Domain;

public class Goal : IEntityBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid user_id { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal goal_amount { get; set; }
    public DateTime Deadline { get; set; }
    public decimal CurrentAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}