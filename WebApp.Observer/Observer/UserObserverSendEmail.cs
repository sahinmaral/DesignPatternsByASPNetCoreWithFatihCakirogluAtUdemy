using System.Net.Mail;
using System.Text.RegularExpressions;
using SendGrid;
using SendGrid.Helpers.Mail;
using WebApp.Observer.Models;

namespace WebApp.Observer.Observer;

public class UserObserverSendEmail(IServiceProvider serviceProvider,IConfiguration configuration) : IUserObserver
{
    public async Task UserCreated(User user)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<UserObserverSendEmail>>();
        var client = new SendGridClient(configuration["MailService:APIKey"]);

        var from = new EmailAddress("sahin.maral@hotmail.com", "Şahin MARAL");
        var to = new EmailAddress(user.Email);
        var subject = "Sitemize hoş geldiniz";
        var body = "<p>Sitemizin genel kuralları saygılı olmaktır. </p>";
        
        var strippedHtmlContent = StripHtmlTags(body);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, strippedHtmlContent, body);
        await client.SendEmailAsync(msg);
        
        logger.LogInformation($"Email was send to user => {user.UserName}");
    }
    
    private string StripHtmlTags(string input)
    {
        return Regex.Replace(input, "<.*?>", string.Empty);
    }
}