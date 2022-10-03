using Domain.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.IRepositories;
using Repository.Repositories;
using Repository.UnitOfWork;
using Service.IServices;
using Service.Mapping;
using Service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
// Add services to the container.
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/Authentification/Login");
});

builder.Services.AddScoped<IAuthentificationService, AuthentificationService>();
builder.Services.AddScoped<IAuthentificationRepository, AuthentificationRepository>();
builder.Services.AddScoped<ICommandeRepository, CommandeRepository>();
builder.Services.AddScoped<ICommandeService, CommandeService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
