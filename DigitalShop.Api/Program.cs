using DigitalShop.Application.Helpers;
using DigitalShop.Infrastructure.Repo;
using DigitalShop.Infrastructure.Repo.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace DigitalShop.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ==========================================
            // PHASE 1: THE BUILDER (Dependency Injection)
            // Its the same as doing app = FastAPI() in Python.
            // It creates an instance of the application and prepares it for configuration.
            // ==========================================
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            // Adding JWT authentication to your Swagger documentation
            // not only enhances security but also provides a seamless experience for API users.
            builder.Services.AddAuthentication(cfg => {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["ApplicationSettings:JWT_Secret"]
                            ?? throw new InvalidOperationException("JWT_Secret is not configured.")
                        )
                    ),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<AuthHelpers>();

            // Registers Entity Framework Core and configures DataContext to use SQLite.
            // It reads the database location from the "DefaultConnection" string in appsettings.json.
            builder.Services.AddDbContext<Infrastructure.Data.DataContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ==========================================
            // PHASE 2: THE APP (Middleware Pipeline)
            // Here you define exactly how each HTTP request
            // is processed when the user sends it.
            // ==========================================
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Automatically redirects all HTTP requests to secure HTTPS.
            app.UseHttpsRedirection();

            // Reads token
            app.UseAuthentication();

            // Checks if the incoming request has valid credentials (auth tokens).
            app.UseAuthorization();

            // Maps incoming HTTP URLs to their corresponding Controller methods.
            app.MapControllers();

            // Starts the server and begins listening for incoming requests.
            app.Run();
        }

        // ================================================
        // To really understand what these app.Use... calls (like app.UseAuthorization())
        // are, think of them as a ribbon in a factory.
        // When you send a GET request from the browser, that request enters the pipeline
        // and passes through each of these filters in turn. If a filter rejects the request
        // (eg you are not logged in and UseAuthorization asks for a token),
        // the request is immediately aborted and returned to the user with an error,
        // without ever reaching your C# logic in the controller.
        // ================================================
    }
}
