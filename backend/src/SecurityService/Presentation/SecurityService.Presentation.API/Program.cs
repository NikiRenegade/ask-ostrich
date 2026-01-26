using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using RabbitMQ.Client;
using SecurityService.Application.Interfaces;
using SecurityService.Application.Services;
using SecurityService.Application.Services.TelegramAuth;
using SecurityService.Domain.Entities;
using SecurityService.Domain.Interfaces.Publishers;
using SecurityService.Domain.Interfaces.Repositories;
using SecurityService.Infrastructure.EntityFramework.Contexts;
using SecurityService.Infrastructure.Messaging;
using SecurityService.Infrastructure.Repositories;
var builder = WebApplication.CreateBuilder(args);

// ===== DbContext =====
var connectionString = builder.Configuration.GetConnectionString("authdbconnection");
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(connectionString));

// ===== Identity =====
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

// ===== Authentication (Google + Cookies + JWT Bearer) =====
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddJwtBearer(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
    };
});


// ===== Authorization =====
builder.Services.AddAuthorization();

// ===== Controllers + OpenAPI =====
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// ===== Application Services =====
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<ITelegramAuthService, InMemoryTelegramAuthService>();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddScoped<IEmailConfirmationService, EmailConfirmationService>();
builder.Services.AddScoped<IExternalAuthService, ExternalAuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// ===== Repositories =====
builder.Services.AddScoped<IUserRepository, UserRepository>();

// ===== RabbitMQ =====
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

    var connection = await factory.CreateConnectionAsync();
    return connection;
});

builder.Services.AddSingleton(async sp =>
{
    var connection = await sp.GetRequiredService<Task<IConnection>>();
    var channel = await connection.CreateChannelAsync();
    return channel;
});
builder.Services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();
builder.Services.AddScoped<IUserEventPublisher, RabbitMqUserEventPublisher>();

var app = builder.Build();

// ===== OpenAPI =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ===== Middleware =====
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpMetrics();

app.MapControllers();
app.MapMetrics();

app.Run();
