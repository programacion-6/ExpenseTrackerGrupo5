using Api.Domain;

using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMappingForUsers();
        CreateMappingForExpenses();
        CreateMappingForRecurrentExpenses();
        CreateMappingForIncomes();
        CreateMappingForBudgets();
        CreateMappingForGoals();
    }

    private void CreateMappingForUsers()
    {
        CreateMap<User, UserResponse>();
        CreateMap<CreateUserRequest, User>();
        CreateMap<UpdateUserRequest, User>();
    }

    private void CreateMappingForExpenses()
    {
        CreateMap<CreateExpenseRequest, Expense>();
        CreateMap<UpdateExpenseRequest, Expense>();
        CreateMap<Expense, ExpenseResponse>();
    }

    private void CreateMappingForRecurrentExpenses()
    {
        CreateMap<CreateRecurrentExpenseRequest, RecurrentExpense>();
        CreateMap<UpdateRecurrentExpenseRequest, RecurrentExpense>();
        CreateMap<RecurrentExpense, RecurrentExpenseResponse>();
    }

    private void CreateMappingForIncomes()
    {
        CreateMap<CreateIncomeRequest, Income>();
        CreateMap<UpdateIncomeRequest, Income>();
        CreateMap<Income, IncomeResponse>();
    }

    private void CreateMappingForBudgets()
    {
        CreateMap<CreateBudgetRequest, Budget>();
        CreateMap<UpdateBudgetRequest, Budget>();
        CreateMap<Budget, BudgetResponse>();
    }

    private void CreateMappingForGoals()
    {
        CreateMap<CreateGoalRequest, Goal>();
        CreateMap<UpdateGoalRequest, Goal>();
        CreateMap<Goal, GoalResponse>();
    }
}