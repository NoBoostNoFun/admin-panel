using System;
using AdminPanel;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AdminPanel.Areas.Identity;
using AdminPanel.Cache;
using AdminPanel.Data;
using AdminPanel.DummyApi;
using AdminPanel.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Host.ConfigureServices(services =>
{
    services
        .Configure<DummyApiOptions>(configuration.GetSection(nameof(DummyApiOptions)))
        .AddSingleton<DummyApiHeaderHandler>()
        .AddHttpClient<IDummyApiClient, DummyApiClient>((p, o) =>
        {
            var options = p.GetRequiredService<IOptions<DummyApiOptions>>().Value;
            o.BaseAddress = new Uri(options.BaseUrl);
        })
        .AddHttpMessageHandler<DummyApiHeaderHandler>();

    services
        .Configure<UserServiceOptions>(configuration.GetSection(nameof(UserServiceOptions)))
        .AddSingleton<IUserService, UserService>();

    services
        .Configure<UserServiceCacheOptions>(configuration.GetSection(nameof(UserServiceCacheOptions)))
        .Decorate<IUserService, UserServiceCacheDecorator>()
        .AddDistributedMemoryCache();

    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
    services.AddDatabaseDeveloperPageExceptionFilter();
    services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>();
    services.AddRazorPages();
    services.AddServerSideBlazor();
    services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
