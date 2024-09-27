namespace Api.Domain.Services;

public class IncomeService : IIncomeService
{
    private readonly IIncomeRepository _incomeRepository;

    public IncomeService(IIncomeRepository incomeRepository)
    {
        _incomeRepository = incomeRepository;
    }

    public async Task<bool> CreateAsync(Income income)
    {
        return await _incomeRepository.Save(income);
    }

    public async Task<bool> UpdateAsync(Income income)
    {
        return await _incomeRepository.Update(income);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var income = await _incomeRepository.GetById(id);
        if (income != null)
        {
            return await _incomeRepository.Delete(income);
        }
        return false;
    }
    public async Task<Income> GetByIdAsync(Guid id)
    {
        return await _incomeRepository.GetById(id);
    }

    public async Task<IEnumerable<Income>> GetAllAsync()
    {
        return await _incomeRepository.GetAll();
    }
    public async Task<List<Income>> GetIncomesByUserIdAsync(Guid userId)
    {
        return await _incomeRepository.GetIncomesByUserId(userId);
    }
}
