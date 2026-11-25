using AdopcionWeb.Clients;
using InertiaCore.Extensions;
using Vite.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddInertia(options =>
{
    options.RootView = "~/Views/App.cshtml";
});

builder.Services.AddViteServices(options =>
{
    options.Server.PackageDirectory = "React";
    options.Server.AutoRun = true;
    options.Server.UseReactRefresh = true;
    options.Server.Port = 5173;
});

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5136/";
builder.Services.AddHttpClient<EspeciesApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseViteDevelopmentServer(true);
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseInertia();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
