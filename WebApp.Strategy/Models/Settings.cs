namespace WebApp.Strategy.Models;

public class Settings
{
    public static string ClaimDatabaseType = "databaseType";
    public EDatabaseType DatabaseType;
    public EDatabaseType GetDefaultDatabaseType => EDatabaseType.SqlServer;
}