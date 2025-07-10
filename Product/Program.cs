using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Product.Data;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq; // Keep this for .Select()

var builder = WebApplication.CreateBuilder(args);

// Database Configuration - Changed to use SQLite from connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."))); // Added null coalesce for robustness


// Identity Configuration
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// JWT Authentication Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key 'Jwt:Key' not configured in appsettings.")
        ))
    };
});

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(Program));

// Add controllers and API Explorer
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger/OpenAPI Configuration with JWT security
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add JWT authorization to Swagger UI for testing protected endpoints
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
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

// This block now ONLY contains user seeding and development-specific middleware,
// it NO LONGER automatically applies migrations/creates the database.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Developer exception page for detailed errors in development
    app.UseSwagger();               // Swagger UI for API documentation
    app.UseSwaggerUI();             // Swagger UI for API documentation

    // User Seeding Logic: This will run on app startup in development,
    // including when hosted by WebApplicationFactory for tests.
    using (var scope = app.Services.CreateScope())
    {
        var scopedServices = scope.ServiceProvider;
        var userManager = scopedServices.GetRequiredService<UserManager<IdentityUser>>();
        // var dbContext = scopedServices.GetRequiredService<AppDbContext>(); // Not needed here if Migrate() is removed.

        // REMOVED: dbContext.Database.Migrate() or dbContext.Database.EnsureCreated().
        // For production/development, you'd run 'dotnet ef database update' manually
        // or via a dedicated startup/deployment script.
        // For tests, CustomWebApplicationFactory handles this.

        Task.Run(async () =>
        {
            if (await userManager.FindByNameAsync("testUser") == null)
            {
                var user = new IdentityUser { UserName = "testUser", Email = "test@example.com" };
                var result = await userManager.CreateAsync(user, "TestPassword1!");
                if (result.Succeeded)
                {
                    Console.WriteLine("Test user 'testUser' created successfully (Program.cs).");
                }
                else
                {
                    Console.WriteLine($"Failed to create test user (Program.cs): {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine("Test user 'testUser' already exists (Program.cs).");
            }
        }).Wait(); // Keep .Wait() for synchronous startup if needed, or make main async
    }
}

// Authentication and Authorization Middleware (Order is important)
app.UseAuthentication();
app.UseAuthorization();

// Map controllers to routes
app.MapControllers();

// Run the application
app.Run();

// Required for WebApplicationFactory to discover your Program class
public partial class Program { }