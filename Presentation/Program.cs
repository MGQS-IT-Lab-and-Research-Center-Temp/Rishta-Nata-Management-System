using Application.Interfaces;
using Application.Services;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services for database access

builder.Services.AddMySQLServer<RishtanataDbContext>(
    builder.Configuration.GetConnectionString("DefaultConnection")!);

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<RishtanataDbContext>();

// TEMP: commented out locally to unblock build — see IFormApplicationService.cs / FormApplicationService.cs. Do not commit this change.
// builder.Services.AddScoped<IFormApplicationService, FormApplicationService>();
builder.Services.AddScoped<IJamaatPresidentService, JamaatPresidentService>();

var app = builder.Build();

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
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();