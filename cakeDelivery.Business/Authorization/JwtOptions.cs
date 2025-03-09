namespace cakeDelivery.Business.Authorization;

public class JwtOptions
{
    public string SignKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int AccessTokenLifetimeMinutes { get; set; }
    public int RefreshTokenLifetimeDays { get; set; }
}