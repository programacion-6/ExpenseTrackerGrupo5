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
        services.AddScoped<IIncomeRepository, IncomeRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>(); 
    }

    private static void InjectServices(IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<INotifier<EmailContent>, EmailNotifier>();
        services.AddScoped<IIncomeService, IncomeService>();
        services.AddScoped<IExpenseService, ExpenseService>();
    }

    private static void InjectUtilsHandlers(IServiceCollection services)
    {
        services.AddScoped<IHashingHandler, PasswordHashingHandler>();
        services.AddScoped<ITokenHandler, TokenHandler>();
    }
}