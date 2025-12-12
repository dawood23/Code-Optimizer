using Castle.DynamicProxy;
using CodeOptimizer.API.Security;
using CodeOptimizer.Domain.Mapper;
using CodeOptimizer.Domain.Repositories.Auth;
using CodeOptimizer.Domain.Repositories.Metrices;
using CodeOptimizer.Domain.Services.Auth;
using CodeOptimizer.Domain.Services.CodeOptimizer;
using CodeOptimizer.Domain.Services.CodeOptimizer.Factory;
using CodeOptimizer.Domain.Services.Groq;
using CodeOptimizer.Domain.Services.Metrices;
using CodeOptimizer.Domain.Services.Telemetry;
using CodeOptimizer.Infrastructure.Cache;
using CodeOptimizer.Infrastructure.Database;
using CodeOptimizer.Infrastructure.Interceptors;
using CodeOptimizer.Infrastructure.Security.Hashing;
using CodeOptimizer.Infrastructure.Security.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;
using System.Text;

namespace CodeOptimizer.API.Extensions
{
    public static class DependencyRegistrar
    {
       
        extension(IServiceCollection services)
        {
            public void AddDependencies()
            {
                services.AddHttpContextAccessor();
                services.AddMemoryCache(); 

                services.AddSingleton<DataBaseConnection>();
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<JwtTokenService>();
                services.AddSingleton<PasswordHasher>();
                services.AddScoped<IGroqService, GroqService>();
                services.AddSingleton<ILanguageStrategyFactory, LanguageStrategyFactory>();

                services.AddTransient<CodeOptimizationService>();

                services.AddScoped<ICacheService, MemoryCacheService>();

                services.AddScoped<AutoCacheInterceptor>();
                services.AddSingleton<ProxyGenerator>();

                services.AddScoped<ICodeOptimizationService>(provider =>
                {
                    var generator = provider.GetRequiredService<ProxyGenerator>();
                    var impl = provider.GetRequiredService<CodeOptimizationService>();
                    var interceptor = provider.GetRequiredService<AutoCacheInterceptor>();

                    return generator.CreateInterfaceProxyWithTarget<ICodeOptimizationService>(impl, interceptor);
                });

                services.AddScoped<IMetricesService, MetricesService>();
                services.AddScoped<IMetricesRepository, MetricesRepository>();

                services.AddSingleton(new Meter("codeoptimizer.metrics", "1.0.0"));

                services.AddSingleton<IBusinessMetrics, BusinessMetrics>();

            }


            public void AddAutoMapper()
            {
                services.AddAutoMapper(typeof(Program).Assembly, typeof(AutoMapperProfile).Assembly);
            }

            public void AddJwtAuthenticationScheme(IConfiguration Configuration)
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "CompositeAuth";
                    options.DefaultChallengeScheme = "CompositeAuth";
                })
                 .AddJwtBearer("Bearer", o =>
                 {
                     o.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidIssuer = Configuration["Jwt:Issuer"],
                         ValidateAudience = true,
                         ValidAudience = Configuration["Jwt:Audience"],
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                         ValidateLifetime = true
                     };
                 })
                 .AddJwtBearer("OwinJwt", o =>
                 {
                     o.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:OwinKey"])),
                         ValidateIssuer = false,
                         ValidateAudience = false
                     };
                 })
                 .AddScheme<AuthenticationSchemeOptions, CompositeAuthHandler>("CompositeAuth", options => { });

            }

            public void AddCorsPolicy()
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
                });
            }


            public void AddSwaggerAuth()
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "CodeOptimizer API",
                        Version = "v1"
                    });

                    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Description = "Paste your JWT or OWIN token here. (Format: Bearer {token})",
                        Name = "Authorization",
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                        Scheme = "bearer"
                    });

                    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                    {
                        {
                            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                            {
                                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                                {
                                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                });

            }


        }
    }
}
