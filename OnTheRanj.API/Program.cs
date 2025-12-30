using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Core.Interfaces.Services;
using OnTheRanj.Core.Interfaces.Strategies;
using OnTheRanj.Infrastructure.Data;
using OnTheRanj.Infrastructure.Services;
using OnTheRanj.Infrastructure.Services.Implementations;
using OnTheRanj.Infrastructure.Services.Strategies;
using OnTheRanj.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database Configuration - Using SQLite for development on macOS
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString, b => b.MigrationsAssembly("OnTheRanj.API")));

// Unit of Work and Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Business Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectAssignmentService, ProjectAssignmentService>();
builder.Services.AddScoped<ITimesheetService, TimesheetService>();
builder.Services.AddScoped<ITimesheetApprovalService, TimesheetApprovalService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITimesheetApprovalStrategy, DefaultApprovalStrategy>();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// JWT Authentication
var jwtSecret = builder.Configuration["Jwt:SecretKey"] 
    ?? "YourSuperSecretKeyForJWTTokenGenerationWithMinimum256Bits";
var key = Encoding.ASCII.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

// Authorization
builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OnTheRanj Timesheet API",
        Version = "v1",
        Description = "RESTful API for Timesheet Management System",
        Contact = new OpenApiContact
        {
            Name = "OnTheRanj Team",
            Email = "lsranjitha@gmail.com"
        }
    });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnTheRanj API V1");
        c.RoutePrefix = string.Empty; // Swagger at root
    });
}


// Runtime user and timesheet seeding
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    OnTheRanj.API.Data.DbSeeder.SeedUsers(db);
    OnTheRanj.API.Data.DbSeeder.SeedTimesheets(db);
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowAngular");

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
