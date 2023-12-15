using Sentry;
using Spark.NET.Infrastructure.AppSettings.Models;
using Spark.NET.Shared.Entities.DTOs.API.Authentication;


using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spark.NET.Infrastructure.Contexts;
using Spark.NET.Infrastructure.Services.Authentication;
using Spark.NET.Shared.Entities.DTOs.API;
using Spark.NET.Shared.Entities.Models.User;


public interface IUserService
{
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
    Task<ApplicationUser> GetById(string id);
    Task<ServiceResponse> Register(RegisterRequest model);
}

public class UserService : IUserService
{

    private readonly AppSettings _appSettings;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(IOptions<AppSettings> appSettings, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _appSettings = appSettings.Value;
        _context = context;
        _userManager = userManager;
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);

        // return null if user not found
        if (user == null) return null;
        if (!await _userManager.CheckPasswordAsync(user, model.Password)) return null;
        // authentication successful so generate jwt token
        var token = generateJwtToken(user);
        

        return new AuthenticateResponse(user, token);

    }
    
    public async Task<ServiceResponse> Register(RegisterRequest model)
    {
        var user = new ApplicationUser
        {
            UserName =  model.UserName,
            Id = Guid.NewGuid().ToString()
            
        };
        try
        {
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var token = generateJwtToken(user);

                return new ServiceResponse(new { token });
            }
            else
            {
                return new ServiceResponse(ApiErrorMessages.GenericError, true, result);
            }
        }
        catch(Exception e)
        {
            return new ServiceResponse(e.Message, true);
        }
    }
    

    public async Task<ApplicationUser> GetById(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    // helper methods

    private string generateJwtToken(ApplicationUser user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.SecretKeys.JwtSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}