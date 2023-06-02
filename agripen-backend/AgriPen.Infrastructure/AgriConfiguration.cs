namespace AgriPen.Infrastructure;

public class AgriConfiguration
{
    public AuthJwtConfig JwtAuth { get; set; }
}

public class AuthJwtConfig
{
    public string Key { get; set; }
}
