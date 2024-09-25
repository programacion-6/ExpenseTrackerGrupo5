using Npgsql;
using System.Data;
using Api.Domain.Services;
using Api.Domain;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       "Host=localhost;Database=expense_tracker_group_5_postgres;Username=user;Password=password";

builder.Services.AddTransient<IDbConnection>(sp => 
    new NpgsqlConnection(connectionString));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();