namespace WebApp.Template.UserCards;

public class DefaultUserCardTemplate : UserCardTemplate
{
    protected override string SetFooter()
    {
        return string.Empty;
    }

    protected override string SetPicture()
    {
        return $"<img class='card-img-top p-5' src='/images/default-user-picture.png'>";
    }
}