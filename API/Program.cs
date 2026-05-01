using System;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Application.Interfaces;
using Application.Validators;
using Application.DTOs;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

var serverUrl = builder.Configuration["Server:Url"] ?? "http://0.0.0.0:5000";
builder.WebHost.UseUrls(serverUrl);

// Add services to the container
builder.Services.AddControllers();

// Add API versioning and documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext with SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add JWT authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Add CORS
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", corsPolicy =>
    {
        if (allowedOrigins != null && allowedOrigins.Length > 0)
        {
            corsPolicy.WithOrigins(allowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            return;
        }

        corsPolicy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
    });
});

// Add dependency injection
builder.Services.AddScoped<IAdminAuthRepository, AdminAuthRepository>();
builder.Services.AddScoped<IAdminService, AdminAuthService>();
builder.Services.AddScoped<IValidator<AdminLoginRequest>, AdminLoginValidator>();

// Add Service dependencies
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IValidator<CreateServiceRequest>, CreateServiceValidator>();
builder.Services.AddScoped<IValidator<UpdateServiceRequest>, UpdateServiceValidator>();

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add Logging
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI();

// Add CORS middleware
app.UseCors("AllowFrontend");

// Add Exception Handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
