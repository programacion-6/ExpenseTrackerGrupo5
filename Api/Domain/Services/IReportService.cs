namespace Api.Domain;

public interface IReportService
{
    public Task<MonthlySummaryResponse> GetUserMonthlySummary(Guid userId, DateTime month);
    public Task<ExpenseInsightsResponse> GetUserExpenseInsightsResponse(Guid userId);
}