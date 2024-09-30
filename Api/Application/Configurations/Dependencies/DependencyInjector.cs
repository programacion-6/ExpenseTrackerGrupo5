using Api.Domain;

using Api.Domain.Services;

namespace Api.Application;

public static class DependencyInjector
{
    public static void InjectDependencies(this IServiceCollection services)
    {
        InjectUtilsHandlers(services);
        InjectRepositories(services);
        InjectServices(services);
    }

    private static void InjectRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<IIncomeRepository, IncomeRepository>();
        services.AddScoped<IGoalRepository, GoalRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
    }

    private static void InjectServices(IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<INotifier<EmailContent>, EmailNotifier>();
        services.AddScoped<IIncomeService, IncomeService>();
        services.AddScoped<IGoalService, GoalService>();
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped<IBudgetService, BudgetService>();
        services.AddScoped<ITracker<Income, Budget>, IncomeTrackerForBudget>();
        services.AddScoped<ITracker<Expense, Budget>, ExpenseTrackerForBudget>();
    }

    private static void InjectUtilsHandlers(IServiceCollection services)
    {
        services.AddScoped<IHashingHandler, PasswordHashingHandler>();
        services.AddScoped<ITokenHandler, TokenHandler>();
    }
}