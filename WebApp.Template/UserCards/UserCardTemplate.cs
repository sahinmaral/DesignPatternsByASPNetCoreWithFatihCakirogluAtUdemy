using System.Text;
using WebApp.Template.Models;

namespace WebApp.Template.UserCards;

public abstract class UserCardTemplate
{
    protected User User { get; set; }

    public void SetUser(User user)
    {
        User = user;
    }

    public string Build()
    {
        if (User is null)
            throw new ArgumentNullException(nameof(User));
        
        var sb = new StringBuilder();
        sb.Append("<div class='card'>");
        sb.Append(SetPicture());
        sb.Append($@"<div class='card-body'>
                        <h5>{User.UserName}</h5>
                        <p>{User.Description}</p>");
        sb.Append(SetFooter());
        sb.Append("</div>");
        sb.Append("</div>");

        return sb.ToString();
    }
    
    protected abstract string SetFooter();
    protected abstract string SetPicture();
}