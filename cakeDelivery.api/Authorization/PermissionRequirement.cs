using cakeDelivery.Business.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace cakeDelivery.api.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public Permissions Permission { get; }

    public PermissionRequirement(Permissions permission)
    {
        Permission = permission;
    }
}