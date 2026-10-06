
using CafeManagement.API.Application.Services;
using CafeManagement.API.Domain.Interfaces;
using CafeManagement.API.Infrastructure.BackgroundServices;
using CafeManagement.API.Infrastructure.Persistence;
using CafeManagement.API.Infrastructure.Persistence.Repositories;
using CafeManagement.Shared.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Reflection.Metadata;
using System.Text;

namespace CafeManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<CafeManagementDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddScoped<IMCustomerRepository, MCustomerRepository>();
            builder.Services.AddScoped<IMEmployeeRepository, MEmployeeRepository>();
            builder.Services.AddScoped<IMMenuItemRepository, MMenuItemRepository>();
            builder.Services.AddScoped<IMOrderRepository, MOrderRepository>();
            builder.Services.AddScoped<IMTableRepository, MTableRepository>();

            builder.Services.AddScoped<IMCustomerService, MCustomerService>();
            builder.Services.AddScoped<IMEmployeeService, MEmployeeService>();
            builder.Services.AddScoped<IMMenuItemService, MMenuItemService>();
            builder.Services.AddScoped<IMOrderService, MOrderService>();
            builder.Services.AddScoped<IMTableService, MTableService>();

            builder.Services.AddScoped<QrCodeService>();

            builder.Services.AddScoped<IOutboxService, OutboxService>();

            builder.Services.AddControllers();


            // ── JWT Service ────────────────────────────
            builder.Services.AddScoped<JwtService>();

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



            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info.Title = "CafeManagement.API";

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



            builder.Services.AddHttpClient();

            builder.Services.AddHostedService<OutboxProcessorService>();

            var app = builder.Build();

            app.UseStaticFiles();

            app.UseAuthentication();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.WithPreferredScheme("Bearer")
                    .WithTitle("CafeManagement API"); 
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
