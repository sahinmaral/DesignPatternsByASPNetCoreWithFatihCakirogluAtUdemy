using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApp.Template.Models;

namespace WebApp.Template.UserCards;

public class UserCardTagHelper(IHttpContextAccessor httpContextAccessor) : TagHelper
{
    public User User { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        UserCardTemplate userCardTemplate;
        if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            userCardTemplate = new NormalUserTemplate();
        }
        else
        {
            userCardTemplate = new DefaultUserCardTemplate();
        }
        userCardTemplate.SetUser(User);
        output.Content.SetHtmlContent(userCardTemplate.Build());
    }
}