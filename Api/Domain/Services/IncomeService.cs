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
        var income = _incomeRepository.GetById(id);
        return await _incomeRepository.Delete(income);
    }

    public async Task<Income> GetByIdAsync(Guid id)
    {
        return _incomeRepository.GetById(id);
    }

    public async Task<IEnumerable<Income>> GetAllAsync()
    {
        return _incomeRepository.GetAll();
    }
}
