using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using Sehatak.API.Hubs;
using Sehatak.API.Hubs;
using Sehatak.API.Middleware;
using Sehatak.API.Middleware;
using Sehatak.Application.Interfaces;
using Sehatak.Application.Interfaces;
using Sehatak.Application.Interfaces.Features;
using Sehatak.Application.Interfaces.SuperAdminInterface;
using Sehatak.Infrastructure.Data;
using Sehatak.Infrastructure.Data;
using Sehatak.Infrastructure.Security;
using Sehatak.Infrastructure.Security;
using Sehatak.Infrastructure.Services;
using Sehatak.Infrastructure.Services.FeatureService;
using Sehatak.Infrastructure.Services.PatientRegisterAuth;
using Sehatak.Infrastructure.Services.SuperAdminAuth;
using Serilog;
using Serilog;
using System;
using System.Text;
using System.Threading.RateLimiting;

namespace Sehatak.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 0. SERILOG — أول شي قبل أي خدمة
            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    path: "Logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
                ));

            // 1. CONTROLLERS
            builder.Services.AddControllers();

            // 2. SWAGGER
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter your JWT token. Example: Bearer eyJhbGci..."
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id   = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // 3. LOCALIZATION
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "en", "ar" };
                options.SetDefaultCulture("en")
                       .AddSupportedCultures(supportedCultures)
                       .AddSupportedUICultures(supportedCultures);
            });

            // 4. SHARED DATABASE
            builder.Services.AddDbContext<SharedDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("SharedDb"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("SharedDb"))
                )
            );

            // 5. TENANT DATABASE
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<TenantDbContextFactory>();   // بيبني TenantDbContext لمركز معين، يدوياً
            builder.Services.AddScoped<TenantDbContextAccessor>();     // بيجيب TenantDbContext للمركز الحالي من JWT
            

            // 6. SIGNALR
            builder.Services.AddSignalR();

            // 7. CORS
            var allowedOrigins = builder.Configuration
                .GetSection("Cors:AllowedOrigins")
                .Get<string[]>() ?? [];

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("SehatakPolicy", policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // 8. RATE LIMITING
            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("LoginPolicy", limiterOptions =>
                {
                    limiterOptions.PermitLimit = builder.Configuration.GetValue<int>("RateLimiting:LoginPermitLimit");
                    limiterOptions.Window = TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("RateLimiting:LoginWindowSeconds"));
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 0;
                });

                options.AddFixedWindowLimiter("GeneralPolicy", limiterOptions =>
                {
                    limiterOptions.PermitLimit = builder.Configuration.GetValue<int>("RateLimiting:GeneralPermitLimit");
                    limiterOptions.Window = TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("RateLimiting:GeneralWindowSeconds"));
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 0;
                });

                options.OnRejected = async (context, cancellationToken) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    await context.HttpContext.Response.WriteAsJsonAsync(new
                    {
                        status = 429,
                        message = "Too many requests. Please slow down and try again later."
                    }, cancellationToken);
                };
            });

            // 9. JWT AUTHENTICATION
            var jwtKey = builder.Configuration["Jwt:SecretKey"]!;
            var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
            var jwtAudience = builder.Configuration["Jwt:Audience"]!;

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(new
                        {
                            status = 401,
                            message = "Unauthorized. Please login first."
                        });
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(new
                        {
                            status = 403,
                            message = "Forbidden. You do not have permission to access this resource."
                        });
                    }
                };
            });

            // 10. AUTHORIZATION — ROLES
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("SuperAdmin"));
                options.AddPolicy("AdminOrAbove", policy => policy.RequireRole("SuperAdmin", "Admin"));
                options.AddPolicy("MedicalStaff", policy => policy.RequireRole("Admin", "Doctor", "Receptionist", "LabTechnician"));
                options.AddPolicy("DoctorOnly", policy => policy.RequireRole("Doctor"));
                options.AddPolicy("ReceptionistOnly", policy => policy.RequireRole("Receptionist"));
                options.AddPolicy("PatientOnly", policy => policy.RequireRole("Patient"));
            });

            // 11. SERVICES
            builder.Services.AddSingleton<JwtTokenGenerator>();
            builder.Services.AddScoped<IAuditLogService, AuditLogService>();
            builder.Services.AddScoped<TenantMigrationRunner>();
            builder.Services.AddScoped<ISuperAdminAuthService, SuperAdminAuthService>();

            // Auth flow — repositories بدون interface (تستخدم جوا AuthService بس)


            // Auth flow — services مع interface (تُستدعى من API)
            builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IFeatureService, featureService>();

            var app = builder.Build();

            // MIDDLEWARE PIPELINE — الترتيب مهم جداً
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRequestLocalization();
            app.UseCors("SehatakPolicy");
            app.UseRateLimiter();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHub<ChatHubs>("/hubs/chat");

            app.Lifetime.ApplicationStopping.Register(() => Log.CloseAndFlush());

            app.Run();
        }
    }
}
