using Application.Interfaces;
using Application.Interfaces.Identity;
using Application.Services;
using Gateway.Implementation;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using Presentation.Data;
using Presentation.Services.Auth;
using Infrastructure.Services;
using Domain.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services for database access

builder.Services.AddMySQLServer<RishtanataDbContext>(
    builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddScoped<IAqeeqahCertificateService, AqeeqahCertificateService>();
builder.Services.AddScoped<ICertificateService, CertificateService>();
builder.Services.AddScoped<IFormApplicationService, FormApplicationService>();
builder.Services.AddScoped<IMarriageApplicationFormService, MarriageApplicationFormService>();
builder.Services.AddScoped<IBridegroomService, BridegroomService>();



builder.Services.AddScoped<IRishtanataSecretaryService, RishtanataSecretaryService>();

builder.Services.AddScoped<ICookieAuthenticationService, CookieAuthenticationService>();

builder.Services.AddHttpClient<IGatewayHandler, GatewayHandler>();

builder.Services.AddHttpContextAccessor();

// Invitation and email services
builder.Services.AddScoped<IInvitationService, InvitationService>();
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.AddScoped<IInvitationEmailService, InvitationEmailService>();

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
