using Api.Domain;

namespace Api.Application;

public static class DependencyInjector
{
    public static void InjectDependencies(this IServiceCollection services)
    {
        InjectUtilsHandlers(services);
        InjectRepositories(services);
        InjectServices(services);
    }

    public static void InjectRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }

    public static void InjectServices(IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }

    public static void InjectUtilsHandlers(IServiceCollection services)
    {
        services.AddScoped<IHashingHandler, PasswordHashingHandler>();
        services.AddScoped<ITokenHandler, TokenHandler>();
    }
}