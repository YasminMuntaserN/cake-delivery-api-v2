using cakeDelivery.Business.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace cakeDelivery.api.Authorization;

[AttributeUsage(AttributeTargets.Method )]
public class RequirePermissionAttribute : AuthorizeAttribute
{
    public Permissions Permission { get; }

    public RequirePermissionAttribute(Permissions permission) 
        : base(permission.ToString())  
    {
        Permission = permission;
    }
}