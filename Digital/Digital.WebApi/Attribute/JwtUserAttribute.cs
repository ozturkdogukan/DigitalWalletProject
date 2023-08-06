using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

// Kullanıcının jwt tokenini doğrulamak ve bazı parametreleri almak için yazdım.
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class JwtUserAttribute : Attribute, IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Tokenı al
        var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Token doğrulamasını yap
        var jwtHandler = new JwtSecurityTokenHandler();
        if (!jwtHandler.CanReadToken(token))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var jwtToken = jwtHandler.ReadJwtToken(token);
        var claimsIdentity = new ClaimsIdentity(jwtToken.Claims);

        // Tokenın son kullanma tarihini kontrol et
        var expirationDate = jwtToken.ValidTo;
        if (expirationDate < DateTime.UtcNow)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Kullanıcı bilgilerini al
        var userId = claimsIdentity.FindFirst("UserId")?.Value;
        var userRole = claimsIdentity.FindFirst("Role")?.Value;

        // Bilgileri ortak bir yerde sakla
        context.HttpContext.Items["UserId"] = userId;
        context.HttpContext.Items["UserRole"] = userRole;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Gerekirse, işlem sonrasında yapılacak işlemler buraya eklenebilir
    }
}
