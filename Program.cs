using EmployeeApi.Data;
using EmployeeApi.Extensions;
using EmployeeApi.Repository;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using EmployeeApi.ActionFilters;
using AspNetCoreRateLimit;
using EmployeeApi.Contracts;
using EmployeeApi.Utility;
using EmployeeApi.DataTransferObject.Models;

// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddDbContext<EmployeeDbContext>(options =>
    //options.UseSqlServer(builder.Configuration.GetConnectionString("MSSqlDb"), opt => opt.EnableRetryOnFailure()));
    options.UseSqlite(builder.Configuration.GetConnectionString("SqlLite")));

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddScoped<IEmployeeService, EmployeeService>();
    builder.Services.AddScoped<GlobalExceptionHandler>();
    builder.Services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
    builder.Services.AddCustomMediaTypes();
    builder.Services.AddScoped<ValidateMediaTypeAttribute>();
    builder.Services.ConfigureVersioning();
    builder.Services.ConfigureResponseCaching();
    builder.Services.ConfigureHttpCacheHeaders();
    builder.Services.AddScoped<ValidationFilterAttribute>();

    builder.Services.AddMemoryCache();
    builder.Services.ConfigureRateLimitingOptions();
    builder.Services.AddInMemoryRateLimiting();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<EmployeeLinks>();

    //builder.Services.AddControllers();
    builder.Services.ConfigureControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    //builder.Services.AddSwaggerGen();
    builder.Services.ConfigureSwagger();

    // builder.Services.AddAuthentication("BasicAuthentication")
    //             .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
    //             ("BasicAuthentication", null);

    builder.Services.AddAuthentication();
    builder.Services.ConfigureIdentity();
    builder.Services.ConfigureJWT(builder.Configuration);
    builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();
    builder.Services.AddAuthorization();

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(s =>
        {
            s.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API v1");
        });
    }

    app.UseHttpsRedirection();
    app.UseResponseCaching();
    app.UseHttpCacheHeaders();
    app.UseIpRateLimiting();

    app.UseMiddleware<GlobalExceptionHandler>();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
