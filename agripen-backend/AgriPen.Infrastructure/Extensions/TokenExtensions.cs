using AgriPen.Domain.Entities;
using FastEndpoints;
using System.Security.Claims;

namespace AgriPen.Infrastructure.Extensions;
public static class TokenExtensions
{
    public static void PopulateFromUser(this UserPrivileges privilege, User user)
    {
        privilege.Roles.Add(user.Role.GetClaimFromRole());
        privilege.Claims.Add(new(ClaimTypes.NameIdentifier, user.Id.ToString()));
        privilege.Claims.Add(new(ClaimTypes.Name, user.Username));
    }
}
