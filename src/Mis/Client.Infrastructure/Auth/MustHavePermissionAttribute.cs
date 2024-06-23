using csumathboy.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace csumathboy.Client.Infrastructure.Auth;
public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = FSHPermission.NameFor(action, resource);
}