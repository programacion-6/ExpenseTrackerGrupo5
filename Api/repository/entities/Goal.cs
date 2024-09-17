public class Goal
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal GoalAmount { get; set; }
    public DateTime Deadline { get; set; }
    public decimal CurrentAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}