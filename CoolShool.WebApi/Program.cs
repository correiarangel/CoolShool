using CoolShool.Application.Interfaces;
using CoolShool.Application.Services;
using CoolShool.Domain.Interfaces;
using CoolShool.Infrastructure;
using CoolShool.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using CoolShool.WebApi.GraphQL;
using CoolShool.WebApi.GraphQL.Types;
using CoolShool.WebApi.GraphQL.DataLoaders;
using HotChocolate.AspNetCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorOrigin", policy =>
    {
        policy.WithOrigins("https://localhost:7055")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<IFinancialOwnerRepository, FinancialOwnerRepository>();
builder.Services.AddScoped<IPaymentPlanRepository, PaymentPlanRepository>();
builder.Services.AddScoped<IBillingRepository, BillingRepository>();
builder.Services.AddScoped<ICostCenterRepository, CostCenterRepository>();

builder.Services.AddScoped<IFinancialOwnerService, FinancialOwnerService>();
builder.Services.AddScoped<ICostCenterService, CostCenterService>();
builder.Services.AddScoped<IPaymentPlanService, PaymentPlanService>();
builder.Services.AddScoped<IBillingService, BillingService>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<FinancialOwnerType>()
    .AddType<PaymentPlanType>()
    .AddType<BillingType>()
    .AddDataLoader<PaymentPlanBatchDataLoader>()
    /// Crítico: limite de profundidade de queries — previne DoS
    .AddMaxExecutionDepthRule(5)
    /// Crítico: detalhes de exceção apenas em desenvolvimento
    .ModifyRequestOptions(opt =>
        opt.IncludeExceptionDetails = builder.Environment.IsDevelopment())
    /// Crítico: introspecção desabilitada em produção (segurança)
    .DisableIntrospection(!builder.Environment.IsDevelopment());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("AllowBlazorOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGraphQL();

app.Run();
