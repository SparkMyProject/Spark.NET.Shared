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
    Task<ServiceResponse> Authenticate(AuthenticateRequest model);
    Task<ApplicationUser> GetById(string id);
    Task<ServiceResponse> Register(RegisterRequest model);
    Task<ServiceResponse> DisableAccount(DisableAccountRequest model);
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

    public async Task<ServiceResponse> Authenticate(AuthenticateRequest model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);

        // By having separate error messages below, it tells the user that a user might exist, which is a security risk.
        if (user == null) return new ServiceResponse("UserName or Password not found.", true, user);
        if (user.IsDisabled) return new ServiceResponse("User is disabled.", true, user);
        if (!await _userManager.CheckPasswordAsync(user, model.Password)) return new ServiceResponse("UserName or Password is incorrect.", true, user);
        // authentication successful so generate jwt token
        var token = generateJwtToken(user);
        Object payload = new { User = user, Token = token };

        return new ServiceResponse(payload);

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

                return new ServiceResponse("User created successfully.", false, new { token });
            }
            // Get the error messages
            var errorMessages = result.Errors.Select(e => e.Description).ToList();
            return new ServiceResponse(errorMessages.FirstOrDefault(), true, result.Errors); // This is the message that is displayed to the user.
        }
        catch(Exception e)
        {
            return new ServiceResponse(e.Message, true);
        }
    }

    public async Task<ServiceResponse> DisableAccount(DisableAccountRequest model)
    {
        var user = await _userManager.FindByIdAsync(model.Id); // Get Current User Id
        if (user == null) return new ServiceResponse("User not found.", true, user);
        if (user.Id != model.Id) return new ServiceResponse("Id does not match with current account.", true, user);
        if (!await _userManager.CheckPasswordAsync(user, model.Password)) return new ServiceResponse("Password is incorrect.", true, user);
        user.IsDisabled = true;
        await _userManager.UpdateAsync(user);
        return new ServiceResponse("User disabled successfully.", false, user);
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