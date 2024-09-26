using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Application;

public static class SwaggerConfiguration
{
    public static void ConfigSwaggerGen(SwaggerGenOptions options)
    {
        ConfigSwaggerInformation(options);
        ConfigSweaggerBearer(options);
    }

    private static void ConfigSwaggerInformation(SwaggerGenOptions options)
    {
        var swaggerInformation = new OpenApiInfo
        {
            Title = "Expense Tracker API - Group 5",
            Version = "v1"
        };
        options.SwaggerDoc("v1", swaggerInformation);
    }

    private static void ConfigSweaggerBearer(SwaggerGenOptions options)
    {
        var segurityDescription = new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Enter Bearer in this format 'Bearer [space] <token>'",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        };
        options.AddSecurityDefinition("Bearer", segurityDescription);

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
    }
}