using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SecurityService.Application.Interfaces;
using SecurityService.Application.Services;
using SecurityService.Domain.Entities;
using SecurityService.Domain.Interfaces.Repositories;
using SecurityService.Infrastructure.EntityFramework.Contexts;
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

// ===== Authentication (Google + Cookies) =====
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
});

// ===== Authorization =====
builder.Services.AddAuthorization();

// ===== Controllers + OpenAPI =====
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// ===== Application Services =====
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddScoped<IEmailConfirmationService, EmailConfirmationService>();
builder.Services.AddScoped<IExternalAuthService, ExternalAuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// ===== Repositories =====
builder.Services.AddScoped<IUserRepository, UserRepository>();

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

app.MapControllers();

app.Run();
