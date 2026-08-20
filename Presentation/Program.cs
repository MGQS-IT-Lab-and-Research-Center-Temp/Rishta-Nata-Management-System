using Application.Interfaces;
using Application.Interfaces.Identity;
using Application.Services;
using Gateway.Implementation;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using Presentation.Data;
using Presentation.Services.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services for database access

builder.Services.AddMySQLServer<RishtanataDbContext>(
    builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddScoped<IFormApplicationService, FormApplicationService>();
builder.Services.AddScoped<IAqeeqahCertificateService, AqeeqahCertificateService>();
builder.Services.AddScoped<ICertificateService, CertificateService>();
builder.Services.AddScoped<IRishtanataSecretaryService, RishtanataSecretaryService>();
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<IGatewayHandler, GatewayHandler>();

builder.Services.AddScoped<IJamaatMemberService, JamaatMemberService>();
builder.Services.AddScoped<ICookieAuthenticationService, CookieAuthenticationService>();

builder.Services
    .AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else 
{
    app.UseSwagger(); 
    app.UseSwaggerUI(); 
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
// Seed Aqeeqah certificates data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RishtanataDbContext>();

    await dbContext.Database.MigrateAsync();


    await AqeeqahCertificateSeeder.SeedAqeeqahCertificatesAsync(dbContext);
}

app.Run();
