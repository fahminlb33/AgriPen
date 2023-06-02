namespace AgriPenMobile.API;

public class LoginRequest
{
    public string User { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string UserId { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}

public class RefreshTokenRequest
{
    public string UserId { get; set; }
    public string RefreshToken { get; set; }
}
