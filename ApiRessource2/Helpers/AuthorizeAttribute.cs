namespace ApiRessource2.Helpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ApiRessource2.Models;

public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public Role[] Roles { get; set; } = new Role[] { Role.User, Role.Moderator, Role.Administrator, Role.SuperAdministrator};
    public AuthorizeAttribute(params Role[]? roles) 
    { 
        if (roles.Length != 0)
        {
            Roles = roles;
        }
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.Items["User"] as User;
        // Si array.indexOf == -1 cela veut dire que le role de l'utilisateur n'est pas pas roles.
        if (user == null || Array.IndexOf(Roles, user.Role) == -1)
        {
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}