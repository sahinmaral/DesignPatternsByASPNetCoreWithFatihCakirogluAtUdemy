using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Observer.Models;
using WebApp.Observer.Observer;

namespace WebApp.Observer.Controllers;

public class AccountController(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    UserObserverSubject userObserverSubject)
    : Controller
{
    private readonly UserObserverSubject _userObserverSubject = userObserverSubject;

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var hasUser = await userManager.FindByEmailAsync(email);
        if (hasUser is null)
        {
            return View();
        }

        var signInResult = await signInManager.PasswordSignInAsync(hasUser, password, true, false);
        if (!signInResult.Succeeded)
        {
            return View();
        }
        
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(UserCreateViewModel user)
    {
        var newUser = new User()
        {
            UserName = user.UserName,
            Email = user.Email
        };
        var result = await userManager.CreateAsync(newUser,user.Password);
        if (!result.Succeeded)
        {
            ViewBag.Message = result.Errors.ToList().First().Description;
        }
        else
        {
            await  userObserverSubject.NotifyObservers(newUser);
            ViewBag.Message = "Üyelik sistemi başarıyla gerçekleştirildi";
        }
        
        return View();
    }
}

