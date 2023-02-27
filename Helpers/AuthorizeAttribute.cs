namespace ApiRessource2.Helpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ApiRessource2.Models;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (User)context.HttpContext.Items["User"];
        if (user == null)
        {
            // not logged in
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//TODO: a en discuter
public class NewAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public Role[] Roles { get; set; } = new Role[] { Role.User, Role.Moderator, Role.Administrator, Role.SuperAdministrator};
    public NewAuthorizeAttribute(params Role[] roles) 
    { 
        Roles = roles;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (User)context.HttpContext.Items["User"];
        // Si array.indexOf == -1 cela veut dire que le role de l'utilisateur n'est pas pas roles.
        if (user == null || Array.IndexOf(Roles, user.Role) == -1)
        {
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}