using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Sehatak.API.Middleware;
using Sehatak.Infrastructure.Security;

namespace Sehatak.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // 2. SWAGGER

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                // Add JWT to swagger to check the endpoint
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
            // عشان الرسائل تطلع باللغة اللي يختارها المستخدم

            builder.Services.AddLocalization(options =>
                options.ResourcesPath = "Resources"
            );

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "en", "ar" };

                options.SetDefaultCulture("en")
                       .AddSupportedCultures(supportedCultures)
                       .AddSupportedUICultures(supportedCultures);
            });

            // 4. CORS
            // Determine how can call the API ! ... only FrontEnd when i determine it in appsitting
            var allowedOrigins = builder.Configuration
                .GetSection("Cors:AllowedOrigins")
                .Get<string[]>() ?? [];

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("SehatakPolicy", policy =>
                {
                    policy
                        .WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });


            // 5. RATE LIMITING

            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("LoginPolicy", limiterOptions =>
                {
                    limiterOptions.PermitLimit = builder.Configuration
                        .GetValue<int>("RateLimiting:LoginPermitLimit");

                    limiterOptions.Window = TimeSpan.FromSeconds(
                        builder.Configuration.GetValue<int>("RateLimiting:LoginWindowSeconds")
                    );

                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 0;
                });

                options.AddFixedWindowLimiter("GeneralPolicy", limiterOptions =>
                {
                    limiterOptions.PermitLimit = builder.Configuration
                        .GetValue<int>("RateLimiting:GeneralPermitLimit");

                    limiterOptions.Window = TimeSpan.FromSeconds(
                        builder.Configuration.GetValue<int>("RateLimiting:GeneralWindowSeconds")
                    );

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

            // 6. JWT AUTHENTICATION
            
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

            // 7. AUTHORIZATION — ROLES
            builder.Services.AddAuthorization(options =>
            {
                //  SuperAdmin
                options.AddPolicy("SuperAdminOnly",
                    policy => policy.RequireRole("SuperAdmin"));

                // SuperAdmin or Admin
                options.AddPolicy("AdminOrAbove",
                    policy => policy.RequireRole("SuperAdmin", "Admin"));

                // Staff
                options.AddPolicy("MedicalStaff",
                    policy => policy.RequireRole("Admin", "Doctor", "Receptionist", "LabTechnician"));

                // Only Doctor
                options.AddPolicy("DoctorOnly",
                    policy => policy.RequireRole("Doctor"));

                // Only Patient
                options.AddPolicy("PatientOnly",
                    policy => policy.RequireRole("Patient"));
            });

            // 8. SERVICES
            
            // Singleton — ينشأ مرة وحدة طول عمر التطبيق
            builder.Services.AddSingleton<JwtTokenGenerator>();

            
            var app = builder.Build();

            // MIDDLEWARE PIPELINE — الترتيب مهم جداً

            // 1. Exception Middleware — أول شي عشان يمسك كل الأخطاء
            app.UseMiddleware<ExceptionMiddleware>();

            // 2. Swagger — بس بالـ Development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // 3. HTTPS
            app.UseHttpsRedirection();

            // 4. Localization — قبل Authentication عشان رسائل الأخطاء تطلع باللغة الصح
            app.UseRequestLocalization();

            // 5. CORS — قبل Authentication
            app.UseCors("SehatakPolicy");

            // 6. Rate Limiting — قبل Authentication
            app.UseRateLimiter();

            // 7. Authentication — من هو المستخدم
            app.UseAuthentication();

            // 8. Authorization — عنده صلاحية
            app.UseAuthorization();

            // 9. Controllers
            app.MapControllers();

            app.Run();
        }
    }
}
