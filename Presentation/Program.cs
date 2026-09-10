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

// Validation is driven by explicit [Required] attributes only. Non-nullable
// reference types (NRT) in the view models would otherwise get an implicit
// [Required] that: (1) blocks client-side form submission for fields without
// rendered validation spans (Genotype, BloodGroup, remarriage radios, divorce
// evidence — no asp-validation-for), producing a silently-disabled
// "Save & Continue" button; and (2) fails server-side ModelState invisibly,
// re-rendering the form with no visible error. See docs/bugs-and-gaps.md.
builder.Services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

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