using System.Net.Mime;
using System.Text.RegularExpressions;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace WebApp.ChainOfResponsibility.ChainOfResponsibility;

public class SendEmailProcessorHandler:ProcessHandler
{
    private readonly IConfiguration _configuration;
    private readonly string _fileName;
    private readonly string _toEmail;

    public SendEmailProcessorHandler(string fileName, string toEmail, IConfiguration configuration)
    {
        _fileName = fileName;
        _toEmail = toEmail;
        _configuration = configuration;
    }

    public override object Handle(object o)
    {
        var zipMemoryStream = o as MemoryStream;
        zipMemoryStream.Position = 0;
        var client = new SendGridClient(_configuration["MailService:APIKey"]);

        var from = new EmailAddress("sahin.maral@hotmail.com", "Şahin MARAL");
        var to = new EmailAddress(_toEmail);
        var subject = "Zip dosyası";
        var body = "<p>Zip dosyası ektedir. </p>";
        
        
        var strippedHtmlContent = StripHtmlTags(body);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, strippedHtmlContent, body);
        msg.AddAttachment(_fileName, Convert.ToBase64String(zipMemoryStream.ToArray()), "application/zip");
        client.SendEmailAsync(msg).Wait();
        
        return base.Handle(o);
    }
    
    private string StripHtmlTags(string input)
    {
        return Regex.Replace(input, "<.*?>", string.Empty);
    }
}