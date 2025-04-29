using Microsoft.AspNetCore.Identity;
using SouthAmp.Infrastructure.Identity;
using System.Text;
using AspNetCoreRateLimit;
using Microsoft.EntityFrameworkCore;
using SouthAmp.Infrastructure.Data;
using SouthAmp.Core.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using SouthAmp.Web.Models;
using SouthAmp.Web.Middleware;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SouthAmp.Application.Interfaces;
using SouthAmp.Application.UseCases;
using SouthAmp.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<IHotelUseCases, HotelUseCases>();
builder.Services.AddScoped<IRoomUseCases, RoomUseCases>();
builder.Services.AddScoped<IPaymentUseCases, PaymentUseCases>();
builder.Services.AddScoped<IReviewUseCases, ReviewUseCases>();
builder.Services.AddScoped<INotificationUseCases, NotificationUseCases>();
builder.Services.AddScoped<IReportUseCases, ReportUseCases>();
builder.Services.AddScoped<IDiscountCodeUseCases, DiscountCodeUseCases>();
builder.Services.AddScoped<ILocationUseCases, LocationUseCases>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IDiscountCodeRepository, DiscountCodeRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IReservationUseCases, ReservationUseCases>();
builder.Services.AddScoped<IAdminUseCases, AdminUseCases>();

// Rate limiting (AspNetCoreRateLimit)
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();

// Identity
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.PostgreSQL(
        connectionString: connectionString,
        tableName: "logs",
        needAutoCreateTable: true,
        columnOptions: null // default columns
    )
    .Enrich.FromLogContext()
    .CreateLogger();

Log.Logger = logger;
builder.Host.UseSerilog();

// Poprawka: użyj ILoggerFactory do loggera Microsoft.Extensions.Logging
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSerilog();
});
var msLogger = loggerFactory.CreateLogger("JwtKeyProvider");

// JWT
var jwtKey = JwtKeyProvider.GetJwtKey(builder.Configuration, msLogger);
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "SouthAmp";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "SouthAmpAudience";
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
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
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = key
    };
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SouthAmp API", Version = "v1", Description = "All endpoints require JWT authentication unless marked as [AllowAnonymous]." });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
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
    // c.EnableAnnotations(); // Usunięto, jeśli nie masz Swashbuckle.Annotations
    c.CustomSchemaIds(type => type.FullName);
    // Add XML comments if available
    var xmlFile = $"SouthAmp.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

try
{
    Log.Information("Starting SouthAmp API host");
    var app = builder.Build();

    // SEED ROLES
    using (var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        foreach (var roleName in Enum.GetNames(typeof(UserRole)))
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new AppRole { Name = roleName });
            }
        }
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseIpRateLimiting();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<ExceptionMiddleware>();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}