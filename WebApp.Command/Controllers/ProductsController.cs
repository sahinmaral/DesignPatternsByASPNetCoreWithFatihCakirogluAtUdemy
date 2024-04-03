using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Command.Commands;
using WebApp.Command.Models;

namespace WebApp.Command.Controllers
{
    public class ProductsController(AppIdentityDbContext dbContext) : Controller
    {
        // GET: ProductsController
        public async Task<ActionResult> Index()
        {
            return View(await dbContext.Products.ToListAsync());
        }

        public async Task<IActionResult> CreateFile(int type)
        {
            var products = await dbContext.Products.ToListAsync();

            FileCreateInvoker fileCreateInvoker = new();
            
            EFileType fileType = (EFileType)type;
            switch (fileType)
            {
                case EFileType.Excel:
                    ExcelFile<Product> excelFile = new ExcelFile<Product>(products);
                    fileCreateInvoker.SetCommand(new CreateExcelTableActionCommand<Product>(excelFile));
                    break;
                case EFileType.Pdf:
                    PdfFile<Product> pdfFile = new PdfFile<Product>(products, HttpContext);
                    fileCreateInvoker.SetCommand(new CreatePdfTableActionCommand<Product>(pdfFile));
                    break;
            }

            return fileCreateInvoker.CreateFile();
        }

        public async Task<IActionResult> CreateFiles()
        {
            var products = await dbContext.Products.ToListAsync();

            FileCreateInvoker fileCreateInvoker = new();
            ExcelFile<Product> excelFile = new ExcelFile<Product>(products);
            PdfFile<Product> pdfFile = new PdfFile<Product>(products, HttpContext);
            
            fileCreateInvoker.AddCommand(new CreateExcelTableActionCommand<Product>(excelFile));
            fileCreateInvoker.AddCommand(new CreatePdfTableActionCommand<Product>(pdfFile));
            var filesResult = fileCreateInvoker.CreateFiles();

            using (var zipMemoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(zipMemoryStream,ZipArchiveMode.Create))
                {
                    foreach (var fileResult in filesResult)
                    {
                        var fileContent = fileResult as FileContentResult;
                        var zipFile = archive.CreateEntry(fileContent.FileDownloadName);

                        using (var zipEntryStream = zipFile.Open())
                        {
                            await new MemoryStream(fileContent.FileContents).CopyToAsync(zipEntryStream);
                        }
                    }
                }

                return File(zipMemoryStream.ToArray(), "application/zip", "all.zip");
            }
        }
    }
}
