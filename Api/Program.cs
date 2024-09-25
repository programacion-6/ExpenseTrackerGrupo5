using System.Data;

using Api.Application;

using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtConfiguration.ConfigAuthentication)
                .AddJwtBearer(JwtConfiguration.ConfigBearer);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerConfiguration.ConfigSwaggerGen);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
builder.Services.AddValidators();
builder.Services.InjectDependencies();
builder.Services.AddTransient<IDbConnection>(
                DatabaseConnecctionConfiguration.ConfigDatabaseConnection);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
