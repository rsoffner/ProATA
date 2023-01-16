using System.Globalization;
using Microsoft.AspNetCore.Localization;
using ApiManager._keenthemes;
using ApiManager._keenthemes.libs;
using Microsoft.EntityFrameworkCore;
using TaskProcessing.Data.Models;
using ApiManager.Services;
using askProcessing.Core.Services.SignalProcessor;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Repositories;
using ApiManager.Services.SignalProcessor;
using ApiManager.Hubs;

var appConfig = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json", false)
	.Build();

var connectionString = appConfig.GetConnectionString("db1");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProATADbContext>
			(options => options.UseSqlServer(connectionString));

builder.Services.AddDistributedMemoryCache();  
builder.Services.AddSession(options => {  
    options.IdleTimeout = TimeSpan.FromMinutes(1);
});
builder.Services.AddScoped<IKTTheme, KTTheme>();
builder.Services.AddSingleton<IKTBootstrapBase, KTBootstrapBase>();

builder.Services.AddScoped<ILogRepository, SqlLogRepository>();
builder.Services.AddScoped<ITaskRepository, SqlTaskRepository>();
builder.Services.AddScoped<ISchedulerRepository, SqlSchedulerRepository>();
builder.Services.AddScoped<IScheduleRepository, SqlScheduleRepository>();
builder.Services.AddScoped<IUserRepository, SqlUserRepository>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<ISignalProcessorManager, SignalProcessorManager>();

builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        var supportedCultures = new[] {
            new CultureInfo("ja"),
            new CultureInfo("en"),
            new CultureInfo("ar"),
            new CultureInfo("de"),
            new CultureInfo("es"),
            new CultureInfo("fr"),
        };
        options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
    });

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("themesettings.json")
                            .Build();

var app = builder.Build();

app.Use(async (context, next) =>
    {
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/not-found";
        await next();
    }
});

KTThemeSettings.init(configuration);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.UseThemeMiddleware();
app.MapControllerRoute(name: "signin",
                pattern: "signin",
                defaults: new { controller = "Auth", action = "signIn" });
app.MapControllerRoute(name: "signup",
                pattern: "signup",
                defaults: new { controller = "Auth", action = "signUp" });
app.MapControllerRoute(name: "reset-password",
                pattern: "reset-password",
                defaults: new { controller = "Auth", action = "resetPassword" });
app.MapControllerRoute(name: "new-password",
                pattern: "new-password",
                defaults: new { controller = "Auth", action = "newPassword" });

app.MapControllerRoute(name: "not-found",
                pattern: "not-found",
                defaults: new { controller = "System", action = "notFound" });
app.MapControllerRoute(name: "error",
                pattern: "error",
                defaults: new { controller = "System", action = "error" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Monitor}/{action=Index}/{id?}");

app.MapHub<MessageBrokerHub>("/messagebroker");

app.Run();
