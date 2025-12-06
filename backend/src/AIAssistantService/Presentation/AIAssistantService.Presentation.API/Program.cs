using AIAssistantService.Application.Services;
using AIAssistantService.Domain.Interfaces.Services;
using AIAssistantService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register services
builder.Services.AddScoped<ILLMClientService, LLMClientService>();
builder.Services.AddScoped<ILLMChatApiService, OllamaApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
