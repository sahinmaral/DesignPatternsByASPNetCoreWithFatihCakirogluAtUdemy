using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Adapter.Models;
using WebApp.Adapter.Services;

namespace WebApp.Adapter.Controllers;

public class HomeController(IImageProcess imageProcess) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult AddWatermark()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddWatermark(IFormFile image)
    {
        if (image.Length >= 0)
        {
            var imageMemoryStream = new MemoryStream();
            await image.CopyToAsync(imageMemoryStream);
            imageProcess.AddWatermark("ASP.Net Core MVC",image.FileName, imageMemoryStream);
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}