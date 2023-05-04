using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Spark.NET.Infrastructure.AppSettings.Models;
using Spark.NET.Shared.Entities.Models.User;

namespace Spark.NET.Infrastructure.Services.Authentication;

public interface IJwtUtils
{
    public string GenerateJwtToken(Shared.Entities.Models.User.User user);
    public int? ValidateJwtToken(string? token);
}

public class JwtUtils : IJwtUtils
{
    private readonly SecretKeysSettings _secretKeysSettings;

    public JwtUtils(IOptions<SecretKeysSettings> secretKeysSettings)
    {
        _secretKeysSettings = secretKeysSettings.Value;

        if (string.IsNullOrEmpty(_secretKeysSettings.JwtSecret))
            throw new Exception("JWT secret not configured");
    }

    public string GenerateJwtToken(Shared.Entities.Models.User.User user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKeysSettings.JwtSecret!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public int? ValidateJwtToken(string? token)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKeysSettings.JwtSecret!);
        try
        {
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
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            // return user id from JWT token if validation successful
            return userId;
        }
        catch
        {
            // return null if validation fails
            return null;
        }
    }
}