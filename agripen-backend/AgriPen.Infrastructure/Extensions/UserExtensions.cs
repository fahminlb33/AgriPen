using AgriPen.Domain.Enums;
using System.Security.Claims;

namespace AgriPen.Infrastructure.Extensions;

public static class UserExtensions
{
    public const string Admin = "admin";
    public const string NormalUser = "user";

    public static readonly string[] LoggedInRole = new[] { Admin, NormalUser };

    public static string GetClaimFromRole(this UserRole role)
    {
        return role switch
        {
            UserRole.Administrator => Admin,
            UserRole.Normal => NormalUser,
            _ => ""
        };
    }

    public static UserRole GetRoleFromClaims(this IEnumerable<Claim> claims)
    {
        throw new NotImplementedException();
    }
}
