namespace Api.Domain.Services;

public interface IIncomeService
{
    public Task<bool> CreateAsync(Income income);
    public Task<bool> UpdateAsync(Income income);
    public Task<bool> DeleteAsync(Guid id);
    public Task<Income> GetByIdAsync(Guid id);
    public Task<IEnumerable<Income>> GetAllAsync();
}
