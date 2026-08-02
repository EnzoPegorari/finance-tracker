namespace FinanceTracker.Api.Helpers;

public static class ConnectionStringHelper
{
    /// <summary>
    /// Render (and most PaaS providers) hand out Postgres connections as a
    /// "postgres://user:pass@host:port/db" URI. Npgsql expects ADO.NET keyword=value
    /// syntax, so this converts the URI form when present and passes anything else through.
    /// </summary>
    public static string? Resolve(string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            return connectionString;

        if (!connectionString.StartsWith("postgres://") && !connectionString.StartsWith("postgresql://"))
            return connectionString;

        var uri = new Uri(connectionString);
        var userInfo = uri.UserInfo.Split(':', 2);

        return $"Host={uri.Host};Port={(uri.Port > 0 ? uri.Port : 5432)};Database={uri.AbsolutePath.TrimStart('/')};" +
               $"Username={Uri.UnescapeDataString(userInfo[0])};Password={Uri.UnescapeDataString(userInfo.Length > 1 ? userInfo[1] : string.Empty)};" +
               "SSL Mode=Require;Trust Server Certificate=true";
    }
}
