using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StudentRegistrationSystem.Core.Interfaces;
using StudentRegistrationSystem.Web.ViewModels.Account;

namespace StudentRegistrationSystem.Web.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _authService.LoginAsync(model.Username, model.Password);
        if (user == null)
        {
            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            await _authService.RegisterAsync(
                model.FullName,
                model.Username,
                model.Password,
                model.Email,
                model.Phone,
                model.AcademicYear
            );
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult ForgotPassword() => View();

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await _authService.SendPasswordResetEmailAsync(model.Email);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult ResetPassword(string token)
    {
        var model = new ResetPasswordViewModel { Token = token };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        
        try
        {
            var result = await _authService.ResetPasswordAsync(model.Token, model.NewPassword);
            if (result)
            {
                return RedirectToAction("Login");
            }
            ModelState.AddModelError("", "Password reset failed.");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
        
        return View(model);
    }

    [HttpGet]
    public IActionResult VerifyEmail(string token)
    {
        var model = new VerifyEmailViewModel { Token = token };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        
        var result = await _authService.VerifyEmailAsync(model.Token);
        if (result)
        {
            return RedirectToAction("Login");
        }
        
        ModelState.AddModelError("", "Email verification failed.");
        return View(model);
    }

    [HttpGet]
    public IActionResult AccessDenied() => View();
}
