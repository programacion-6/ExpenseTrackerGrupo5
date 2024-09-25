using System.Data;

using Api.Application;

using FluentValidation.AspNetCore;

using Api.Domain;

using Npgsql;

using Npgsql;
using System.Data;
using Api.Domain.Services;
using Api.Domain;

var builder = WebApplication.CreateBuilder(args);

/* var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       "Host=localhost;Database=expense_tracker_group_5_postgres;Username=user;Password=password";

builder.Services.AddTransient<IDbConnection>(sp => 
    new NpgsqlConnection(connectionString));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddSingleton(new BaseContext(connectionString)); */

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtConfiguration.ConfigAuthentication)
                .AddJwtBearer(JwtConfiguration.ConfigBearer);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerConfiguration.ConfigSwaggerGen);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSwaggerGen();
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
