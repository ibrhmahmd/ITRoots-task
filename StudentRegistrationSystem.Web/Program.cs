using Microsoft.AspNetCore.Authentication.Cookies;
using StudentRegistrationSystem.Core.Interfaces;
using StudentRegistrationSystem.Core.Services;
using StudentRegistrationSystem.Data.Context;
using StudentRegistrationSystem.Data.Repositories;
using StudentRegistrationSystem.Domain.Common;
using StudentRegistrationSystem.Domain.Interfaces;
using StudentRegistrationSystem.Web.Extensions;
using StudentRegistrationSystem.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddViewLocalization();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddScoped<IDbConnectionFactory, DapperContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddSingleton(builder.Configuration.GetSection("AppSettings").Get<AppSettings>() ?? new AppSettings());

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IStudentService, StudentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCustomExceptionHandling();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCustomRequestLogging();
app.UseRouting();
app.ConfigureLocalization();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
