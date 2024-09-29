namespace Api.Domain;

public interface IReportService
{
    public Task<MonthlySummaryResponse> GetUserMonthlySummary(Guid userId);
    public Task<ExpenseInsightsResponse> GetUserExpenseInsightsResponse(Guid userId);
}