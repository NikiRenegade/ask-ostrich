using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using SurveyResponseService.Application.Services;
using SurveyResponseService.Domain.Interfaces.Repositories;
using SurveyResponseService.Domain.Interfaces.Services;
using SurveyResponseService.Domain.Interfaces.Consumers;
using SurveyResponseService.Infrastructure.EntityFramework;
using SurveyResponseService.Infrastructure.Repositories;
using SurveyResponseService.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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

builder.Services.AddDbContext<SurveyResponseDbContext>(options =>
    options.UseMongoDB(mongoConnectionString!, mongoDatabase!));

// Register repositories
builder.Services.AddScoped<ISurveyRepository, SurveyRepository>();
builder.Services.AddScoped<ISurveyResultRepository, SurveyResultRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register services
builder.Services.AddScoped<ISurveyService, SurveyService>();
builder.Services.AddScoped<ISurveyResultService, SurveyResultService>();
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
builder.Services.AddSingleton<IEventConsumer, RabbitMqEventConsumer>();
builder.Services.AddScoped<IUserEventConsumer, RabbitMqUserEventConsumer>();
builder.Services.AddScoped<ISurveyEventConsumer, RabbitMqSurveyEventConsumer>();
builder.Services.AddHostedService<UserEventsBackgroundService>();
builder.Services.AddHostedService<SurveyEventsBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
