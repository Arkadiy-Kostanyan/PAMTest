using Ardalis.ListStartupServices;
using Microsoft.OpenApi.Models;
using Serilog;
using PAM.Core;
using PAM.Infrastructure.Data;
using PAM.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using PAM.Core.Interfaces;
using PAM.Web.Services;
using PAM.Web.Middleware;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllersWithViews().AddNewtonsoftJson();
builder.Services.AddRazorPages();

builder.Services.AddApiVersioning(o =>
{
  o.AssumeDefaultVersionWhenUnspecified = true;
  o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
  o.ReportApiVersions = true;
  o.ApiVersionReader = ApiVersionReader.Combine(
      new QueryStringApiVersionReader("api-version"),
      new HeaderApiVersionReader("X-Version"),
      new MediaTypeApiVersionReader("ver"));

});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PAM API", Version = "v1" });
    c.EnableAnnotations();
});

// add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
builder.Services.Configure<ServiceConfig>(config =>
{
    config.Services = new List<ServiceDescriptor>(builder.Services);

    // optional - default path to view services is /listallservices - recommended to choose your own path
    config.Path = "/listservices";
});

var app = builder.Build();

// add performace check middleware
app.UsePerformanceCheck();

// add custom global exception handler
app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseShowAllServicesMiddleware();
}
else
{
    app.UseHsts();
}
app.UseRouting();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PAM API V1"));

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
});

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // Comment Migrate line if doing that manually
        context.Database.Migrate();
        context.Database.EnsureCreated();
        SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
    }
}

app.Run();
