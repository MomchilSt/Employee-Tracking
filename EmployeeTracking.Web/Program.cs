using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EmployeeTracking.Data.Data;
using EmployeeTracking.Data.Data.Models;
using EmployeeTracking.Services.Interfaces;
using EmployeeTracking.Services.Services;
using EmployeeTracking.Web.Seeder;
using AutoMapper;
using EmployeeTracking.Web.Mapper;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var connectionString = builder.Configuration.GetConnectionString("EmployeeTrackingDbContextConnection") ?? throw new InvalidOperationException("Connection string 'EmployeeTrackingDbContextConnection' not found.");

builder.Services.Configure<IdentityOptions>(options => 
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
});

builder.Services.AddDbContext<EmployeeTrackingDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<TrackingUser, IdentityRole>()
    .AddEntityFrameworkStores<EmployeeTrackingDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IVacationService, VacationService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<DbInitializer>();

// Auto Mapper Configurations
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MapperConfig());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

DbInitializer.Initialize(app);

app.Run();
