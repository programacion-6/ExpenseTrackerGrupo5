namespace Api.Domain.Services;

public interface IIncomeService
{
    Task<bool> CreateAsync(Income income);
    Task<bool> UpdateAsync(Income income);
    Task<bool> DeleteAsync(Guid id);
    Task<Income> GetByIdAsync(Guid id);
    Task<IEnumerable<Income>> GetAllAsync();
}
