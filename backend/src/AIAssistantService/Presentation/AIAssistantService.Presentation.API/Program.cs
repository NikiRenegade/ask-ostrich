using AIAssistantService.Application.Services;
using AIAssistantService.Domain.Interfaces.Services;
using AIAssistantService.Infrastructure.Services;
using AIAssistantService.Presentation.API.SignalR;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add signalR
builder.Services.AddSignalR();

// Register services
builder.Services.AddScoped<ILLMClientService, LLMClientService>();
builder.Services.AddScoped<ILLMChatApiService, OllamaApiService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); 
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseHttpMetrics();

app.MapControllers();

app.UseCors("CorsPolicy");
app.MapHub<LLMHub>("api/AIAssistant/stream");

app.MapMetrics();

app.Run();
