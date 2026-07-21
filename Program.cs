using Final_Insure.Models;
using Final_Insure.Repositories.Implementations;
using Final_Insure.Repositories.Interfaces;
using Final_Insure.Services.Implementations;
using Final_Insure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // Redirects here if not logged in
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Redirects here if wrong role
    });

// Database connection Details
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConn")));

// Register Specific Repositories for Dependency Injection (MOVED ABOVE BUILD)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
builder.Services.AddScoped<IClaimRepository, ClaimRepository>();
builder.Services.AddScoped<IClaimDocumentRepository, ClaimDocumentRepository>();
builder.Services.AddScoped<IAssessmentRepository, AssessmentRepository>();
builder.Services.AddScoped<IFraudCheckRepository, FraudCheckRepository>();
builder.Services.AddScoped<ISettlementLogRepository, SettlementLogRepository>();

// Register Services (MOVED ABOVE BUILD)
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IAssessmentService, AssessmentService>();
builder.Services.AddScoped<IFraudCheckService, FraudCheckService>();
builder.Services.AddScoped<IPolicyService, PolicyService>();


// ==========================================
// BUILD THE APP (Configuration is now locked)
// ==========================================
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting(); // (Removed the duplicate)

app.UseAuthentication(); // <-- MUST BE BEFORE AUTHORIZATION
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();