using Application.Extensions;
using DotNetEnv;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Presentation.Extensions;

// Load local secrets/overrides from a `.env` file (copy `.env.example` to
// `.env` and fill in real values — see README.md). TraversePath searches the
// current directory and its parents, so a `.env` at the repository root is
// found even when the app is launched from Presentation/ (e.g. Visual Studio).
// If no `.env` exists, Load() returns without error.
Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddPresentationServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RishtanataDbContext>();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}")
    .WithStaticAssets();

app.Run();