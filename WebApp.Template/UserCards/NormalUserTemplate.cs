using System.Text;

namespace WebApp.Template.UserCards;

public class NormalUserTemplate : UserCardTemplate
{
    protected override string SetFooter()
    {
        var sb = new StringBuilder();
        sb.Append("<a href='#' class='card-link'>Mesaj gönder</a>");
        sb.Append("<a href='#' class='card-link'>Detaylı profil</a>");
        return sb.ToString();
    }

    protected override string SetPicture()
    {
        return $"<img class='card-img-top p-5' src='{User.PictureUrl}'>";
    }
}