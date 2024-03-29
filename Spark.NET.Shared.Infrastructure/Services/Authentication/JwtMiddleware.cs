using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Spark.NET.Infrastructure.Services.Authentication;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Infrastructure.AppSettings.Models.AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<Infrastructure.AppSettings.Models.AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IUserService userService)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
       // var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            attachUserToContext(context, userService, token);

        await _next(context);
    }

    private void attachUserToContext(HttpContext context, IUserService userService, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretKeys.JwtSecret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

            // attach user to context on successful jwt validation
            context.Items["User"] = userService.GetById(userId);
        }
        catch
        {
            Console.WriteLine("JwtMiddleware.cs: JwtMiddleware.attachUserToContext(): Jwt validation failed");
            // do nothing if jwt validation fails
            // user is not attached to context so request won't have access to secure routes
        }
    }
}