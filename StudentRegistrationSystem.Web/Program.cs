using Microsoft.AspNetCore.Authentication.Cookies;
using StudentRegistrationSystem.Core.Interfaces;
using StudentRegistrationSystem.Core.Services;
using StudentRegistrationSystem.Data.Context;
using StudentRegistrationSystem.Data.Repositories;
using StudentRegistrationSystem.Domain.Common;
using StudentRegistrationSystem.Domain.Interfaces.Repositories;
using StudentRegistrationSystem.Web.Extensions;
using StudentRegistrationSystem.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddScoped<IDbConnectionFactory, DapperContext>();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddSingleton(builder.Configuration.GetSection("AppSettings").Get<AppSettings>() ?? new AppSettings());

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IEmailService, EmailService>();

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
app.UseCustomLocalization();
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
