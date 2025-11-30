using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using SurveyManageService.Application.Services;
using SurveyManageService.Domain.Interfaces.Consumers;
using SurveyManageService.Domain.Interfaces.Publishers;
using SurveyManageService.Domain.Interfaces.Repositories;
using SurveyManageService.Domain.Interfaces.Services;
using SurveyManageService.Infrastructure.EntityFramework;
using SurveyManageService.Infrastructure.Messaging;
using SurveyManageService.Infrastructure.Repositories;

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

// Подключение к RabbitMQ
var rabbitConfig = builder.Configuration.GetSection("RabbitMQ");

builder.Services.AddSingleton(async sp =>
{
    var factory = new ConnectionFactory
    {
        HostName = rabbitConfig["HostName"],
        Port = int.Parse(rabbitConfig["Port"]!),
        UserName = rabbitConfig["UserName"],
        Password = rabbitConfig["Password"]
    };
    return await factory.CreateConnectionAsync();
});

builder.Services.AddSingleton(async sp =>
{
    var connection = await sp.GetRequiredService<Task<IConnection>>();
    var channel = await connection.CreateChannelAsync();
    return channel;
});
builder.Services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();
builder.Services.AddScoped<ISurveyEventPublisher, RabbitMqSurveyEventPublisher>();
builder.Services.AddSingleton<IEventConsumer, RabbitMqEventConsumer>();
builder.Services.AddScoped<IUserEventConsumer, RabbitMqUserEventConsumer>();
builder.Services.AddHostedService<UserEventsBackgroundService>();

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
