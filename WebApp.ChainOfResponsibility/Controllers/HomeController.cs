using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.ChainOfResponsibility.ChainOfResponsibility;
using WebApp.ChainOfResponsibility.Models;

namespace WebApp.ChainOfResponsibility.Controllers;

public class HomeController(AppIdentityDbContext dbContext,IConfiguration configuration) : Controller
{
    
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> SendEmail()
    {
        var products = await dbContext.Products.ToListAsync();

        ExcelProcessHandler<Product> excelProcessHandler = new();
        ZipFileProcessHandler<Product> zipFileProcessHandler = new();
        SendEmailProcessorHandler sendEmailProcessorHandler = new("product.zip","sahinnmaral@gmail.com",configuration);

        excelProcessHandler
            .SetNext(zipFileProcessHandler)
            .SetNext(sendEmailProcessorHandler);

        excelProcessHandler.Handle(products);
        return View(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}