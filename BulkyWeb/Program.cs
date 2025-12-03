using BulkyBook.DataAccess.DbInitializer;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Data.Sqlite; // Add this line
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.DependencyInjection; // Add this line
using Stripe;
using Microsoft.Extensions.Logging;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

// Increase logging verbosity to help diagnose startup/shutdown reasons
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (!string.IsNullOrEmpty(connectionString) && connectionString.Contains("Data Source="))
{
    //(3) SqliteDB  2025.03.26 11:49
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.ConfigureWarnings(warnings =>

        warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored));
    });
}
else
{
    //(2)MSSQLLocalDB or Azure_MSSQLServerDB 2025.03.26 11:49
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));
}

//if (connectionString.Contains("Data Source="))
//{
//    //(3) SqliteDB  2025.03.26 11:49
//  builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlite(builder.Configuration["ConnectionStrings:DefaultConnection"]));
//}
//else
//{
//    //(2)MSSQLLocalDB or Azure_MSSQLServerDB 2025.03.26 11:49
//    builder.Services.AddDbContext<ApplicationDbContext>(options =>
//        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//}

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
// Register ASP.NET Identity using the application's ApplicationUser type
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
builder.Services.AddAuthentication().AddFacebook(option =>
{
    option.AppId = "193813826680436";
    option.AppSecret = "8fc42ae3f4f2a4986143461d4e2da919";
});
builder.Services.AddAuthentication().AddMicrosoftAccount(option =>
{
    option.ClientId = "ec4d380d-d631-465d-b473-1e26ee706331";
    option.ClientSecret = "qMW8Q~LlEEZST~SDxDgcEVx_45LJQF2cQ_rEKcSQ";
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddControllers(); // <- 必要：啟用 API controllers 2025 12 01
// Allow CORS from the local Vite dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocal", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
var app = builder.Build();

// Add lifecycle logging to help diagnose unexpected shutdowns
var logger = app.Services.GetService<ILogger<Program>>();
logger?.LogDebug("Services built and app instance created");
if (logger != null)
{
    app.Lifetime.ApplicationStarted.Register(() => logger.LogInformation("Application lifetime: Started"));
    app.Lifetime.ApplicationStopping.Register(() => logger.LogWarning("Application lifetime: Stopping"));
    app.Lifetime.ApplicationStopped.Register(() => logger.LogWarning("Application lifetime: Stopped"));
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// existing middleware...
// Note: MapControllers must be called after routing and CORS middleware

app.UseHttpsRedirection();
app.UseStaticFiles();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
app.UseRouting();
// Enable CORS for requests from the frontend dev server
app.UseCors("AllowLocal");
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
// Map attribute-routed API controllers (e.g. /api/auth/login)
app.MapControllers();
logger?.LogDebug("MapControllers() executed");

try
{
    Console.WriteLine("SeedDatabase: starting");
    logger?.LogInformation("SeedDatabase: starting");
    SeedDatabase();
    Console.WriteLine("SeedDatabase: completed");
    logger?.LogInformation("SeedDatabase: completed");
}
catch (Exception ex)
{
    Console.WriteLine("SeedDatabase threw an exception: " + ex);
    logger?.LogError(ex, "SeedDatabase threw an exception");
}
app.MapRazorPages();
logger?.LogDebug("MapRazorPages() executed");

app.MapControllerRoute(
    name: "default",
   // pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}"
    pattern: "{area=Customer}/{controller=Home}/{action=Index1}/{id?}"
    );
try
{
    logger?.LogInformation("About to call app.Run()");
    logger?.LogDebug("Final middleware and endpoints configured, calling app.Run()");
    Console.WriteLine("About to call app.Run()");

    // Global exception handlers to capture unobserved exceptions
    AppDomain.CurrentDomain.UnhandledException += (s, e) =>
    {
        var ex = e.ExceptionObject as Exception;
        Console.WriteLine("Unhandled exception (AppDomain): " + ex);
        logger?.LogCritical(ex, "Unhandled exception (AppDomain)");
    };
    TaskScheduler.UnobservedTaskException += (s, e) =>
    {
        Console.WriteLine("Unobserved task exception: " + e.Exception);
        logger?.LogCritical(e.Exception, "Unobserved task exception");
        e.SetObserved();
    };
    // Register a stopping hook to log a stack trace when shutdown is requested
    app.Lifetime.ApplicationStopping.Register(() =>
    {
        try
        {
            Console.WriteLine("ApplicationStopping triggered. Stack trace:\n" + Environment.StackTrace);
            logger?.LogWarning("ApplicationStopping triggered. Stack trace: {stack}", Environment.StackTrace);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error while logging ApplicationStopping: " + ex);
        }
    });
    app.Lifetime.ApplicationStopped.Register(() => logger?.LogWarning("ApplicationStopped event fired"));
    app.Run();
    logger?.LogInformation("app.Run() has returned");
    Console.WriteLine("app.Run() has returned");
}
catch (Exception ex)
{
    logger?.LogCritical(ex, "Unhandled exception in host run");
    Console.WriteLine("Unhandled exception in host run: " + ex);
    throw;
}
void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            dbInitializer.Initialize();
        }
        catch (Exception ex)
        {
            Console.WriteLine("DbInitializer.Initialize() threw an exception: " + ex);
            throw;
        }
    }
}
