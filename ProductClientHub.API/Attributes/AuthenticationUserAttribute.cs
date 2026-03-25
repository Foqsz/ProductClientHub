using Microsoft.AspNetCore.Mvc;
using ProductClientHub.API.Filters;

namespace ProductClientHub.API.Attributes;

public class AuthenticationUserAttribute : TypeFilterAttribute
{
    public AuthenticationUserAttribute() : base(typeof(AuthenticatedUserFilter))
    {
    }
}
