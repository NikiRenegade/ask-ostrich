using Microsoft.EntityFrameworkCore;
using SurveyManageService.Application.Services;
using SurveyManageService.Domain.Interfaces.Repositories;
using SurveyManageService.Domain.Interfaces.Services;
using SurveyManageService.Infrastructure.EntityFramework.Context;
using SurveyManageService.Infrastructure.Repositories.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MongoDB EF Core DbContext registration
var mongoConnectionString = builder.Configuration.GetSection("MongoDb:ConnectionString").Get<string?>();
var mongoDatabase = builder.Configuration.GetSection("MongoDb:Database").Get<string?>();

if (string.IsNullOrWhiteSpace(mongoConnectionString))
{
    throw new InvalidOperationException("MongoDb:ConnectionString is not configured.");
}
if (string.IsNullOrWhiteSpace(mongoDatabase))
{
    throw new InvalidOperationException("MongoDb:Database is not configured.");
}

builder.Services.AddDbContext<SurveyManageDbContext>(options =>
    options.UseMongoDB(mongoConnectionString!, mongoDatabase!));

// Register repositories
builder.Services.AddScoped<ISurveyRepository, SurveyRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register services
builder.Services.AddScoped<ISurveyService, SurveyService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
