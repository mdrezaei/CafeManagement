
using CafeManagement.Shared.Events;
using MenuAndOrdering.API.Application.Services;
using MenuAndOrdering.API.Domain.Entities;
using MenuAndOrdering.API.Domain.Interfaces;
using MenuAndOrdering.API.Domain.ValueObjects;
using MenuAndOrdering.API.Infrastructure.BackgroundServices;
using MenuAndOrdering.API.Infrastructure.Persistence;
using MenuAndOrdering.API.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text;

namespace MenuAndOrdering.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<MenuAndOrderingDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

            builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();

            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            builder.Services.AddScoped<ITableRepository, TableRepository>();

            builder.Services.AddScoped<ICustomerService, CustomerService>();

            builder.Services.AddScoped<IMenuItemService, MenuItemService>();

            builder.Services.AddScoped<IOrderingService, OrderingService>();

            builder.Services.AddScoped<ITableService, TableService>();


            builder.Services.AddScoped<IOutboxService, OutboxService>();
            builder.Services.AddHttpClient();
            builder.Services.AddHostedService<OutboxProcessorService>();


            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info.Title = "MenuAndOrdering.API";

                    // تعریف SecurityScheme با استفاده از AddComponent
                    var bearerScheme = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = "توکن JWT را وارد کنید",
                        In = ParameterLocation.Header
                    };

                    document.Components ??= new OpenApiComponents();
                    document.AddComponent("Bearer", bearerScheme); // ثبت Scheme در Components

                    return Task.CompletedTask;
                });

                options.AddOperationTransformer((operation, context, cancellationToken) =>
                {
                    operation.Security ??= new List<OpenApiSecurityRequirement>();

                    // به جای document از context.Document استفاده کنید
                    operation.Security.Add(new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = new List<string>()
                    });

                    return Task.CompletedTask;
                });
            });


            // ── Authentication ──────────────────────────
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                        )
                    };
                });

            builder.Services.AddAuthorization();


            var app = builder.Build();

            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.WithPreferredScheme("Bearer")
                    .WithTitle("MenuAndOrdering API");
                });
            }

            

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
