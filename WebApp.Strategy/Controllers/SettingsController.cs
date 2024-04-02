using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Controllers;

[Authorize]
public class SettingsController(UserManager<User> userManager, SignInManager<User> signInManager) : Controller
{
    // GET: SettingsController
    public ActionResult Index()
    {
        var settings = new Settings();
        var desiredClaim = User.Claims.FirstOrDefault(c => c.Type == Settings.ClaimDatabaseType);
        if (desiredClaim is null)
            settings.DatabaseType = settings.GetDefaultDatabaseType;
        else
            settings.DatabaseType = (EDatabaseType)int.Parse(desiredClaim.Value);

        return View(settings);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeDatabase(int databaseType)
    {
        var user = await userManager.FindByNameAsync(User.Identity.Name);

        var newClaim = new Claim(Settings.ClaimDatabaseType, databaseType.ToString());

        var userClaims = await userManager.GetClaimsAsync(user);
        var hasDatabaseTypeClaim = userClaims.FirstOrDefault(c => c.Type == Settings.ClaimDatabaseType);
        if (hasDatabaseTypeClaim is null)
        {
            await userManager.AddClaimAsync(user, newClaim);
        }
        else
        {
            await userManager.ReplaceClaimAsync(user, hasDatabaseTypeClaim, newClaim);
        }

        await signInManager.SignOutAsync();

        var authenticateResult = await HttpContext.AuthenticateAsync();
        await signInManager.SignInAsync(user, authenticateResult.Properties);

        return RedirectToAction(nameof(Index));
    }
}