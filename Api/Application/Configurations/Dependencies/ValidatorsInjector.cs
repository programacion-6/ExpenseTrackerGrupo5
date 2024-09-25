using FluentValidation;

namespace Api.Application;

public static class ValidatorsInjector
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<UserValidator>();
    }
}