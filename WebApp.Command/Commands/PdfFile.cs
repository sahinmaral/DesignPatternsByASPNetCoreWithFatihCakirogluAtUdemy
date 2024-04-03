using System.Text;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Primitives;

namespace WebApp.Command.Commands;

public class PdfFile<T>
{
    public readonly HttpContext Context;
    public readonly List<T> _list;
    public string FileName => $"{typeof(T).Name}.pdf";
    public string FileType => $"application/octet-stream";

    public PdfFile(List<T> list, HttpContext context)
    {
        _list = list;
        Context = context;
    }

    public MemoryStream Create()
    {
        var type = typeof(T);
        var sb = new StringBuilder();
        sb.Append(@$"<html>
                    <head>
                    </head>
                    <body>
                    <div class='text-center'>
                    <h1>{type.Name} tablo</h1>
                    </div>
                    <table class='table table-striped'");

        sb.Append("<tr>");
        foreach (var propertyInfo in type.GetProperties().ToList())
        {
            sb.Append($"<th>{propertyInfo.Name}</th>");
        }
        sb.Append("</tr>");
        
        foreach (var listItem in _list)
        {
            var values = type.GetProperties().Select(propertyInfo => propertyInfo.GetValue(listItem, null)).ToList();
            sb.Append("<tr>");
            foreach (var listItemValue in values)
            {
                sb.Append($"<td>{listItemValue}</td>");
            }

            sb.Append("</tr>");
        }

        sb.Append("</table></body></html>");
        
        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
            },
            Objects = {
                new ObjectSettings() {
                    PagesCount = true,
                    HtmlContent = sb.ToString(),
                    WebSettings = { 
                        DefaultEncoding = "utf-8", 
                        UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/lib/bootstrap/dist/css/bootstrap.css")
                    },
                    HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                }
            }
        };

        var converter = Context.RequestServices.GetRequiredService<IConverter>();
        MemoryStream pdfMemoryStream = new(converter.Convert(doc));
        return pdfMemoryStream;
    }
}