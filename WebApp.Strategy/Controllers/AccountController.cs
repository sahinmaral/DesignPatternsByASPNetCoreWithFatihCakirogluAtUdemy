using BaseProject.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var hasUser = await _userManager.FindByEmailAsync(email);
        if (hasUser is null) return View();

        var signInResult = await _signInManager.PasswordSignInAsync(hasUser, password, true, false);
        if (!signInResult.Succeeded) return View();

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
}