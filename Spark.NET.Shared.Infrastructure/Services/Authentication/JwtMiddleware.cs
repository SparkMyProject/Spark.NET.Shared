using Microsoft.AspNetCore.Http;
using Spark.NET.Infrastructure.Services.User;

namespace Spark.NET.Infrastructure.Services.Authentication;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token);
        if (userId != null)
            // attach user to context on successful jwt validation
            context.Items["User"] = userService.GetById(userId.Value);

        await _next(context);
    }
}