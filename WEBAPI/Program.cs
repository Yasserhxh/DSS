using Domain.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
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
var connectionString = 
    builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//var connstring = "Server=msserversql.database.windows.net;Database=CIMAR_PORTAIL_V2;User Id=sqladmin;password=P@ssw0rd2023**";
    
// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   builder.Configuration.GetConnectionString("DefaultConnection")  ));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString,
        o=>o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllers();
builder.Services.AddControllersWithViews().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddScoped<IAuthentificationService, AuthentificationService>();
builder.Services.AddScoped<ICommandeService, CommandeService>();
builder.Services.AddScoped<IAuthentificationRepository, AuthentificationRepository>();
builder.Services.AddScoped<ICommandeRepository, CommandeRepository>();
builder.Services.AddScoped<IBlobService, AzureBlobService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseHsts();
app.MapControllers();
app.UseStaticFiles();
app.Run();
