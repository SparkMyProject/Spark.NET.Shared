using Sentry;
using Spark.NET.Infrastructure.AppSettings.Models;
using Spark.NET.Shared.Entities.DTOs.API.Authentication;


using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spark.NET.Infrastructure.Services.Authentication;
using Spark.NET.Shared.Entities.Models.User;


public interface IUserService
{
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    IEnumerable<ApplicationUser> GetAll();
    ApplicationUser GetById(int id);
}

public class UserService : IUserService
{
    // users hardcoded for simplicity, store in a db with hashed passwords in production applications
    private List<ApplicationUser> _users = new List<ApplicationUser>
    {
        new ApplicationUser { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
    };

    private readonly AppSettings _appSettings;

    public UserService(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

        // return null if user not found
        if (user == null) return null;

        // authentication successful so generate jwt token
        var token = generateJwtToken(user);
        

        return new AuthenticateResponse(user, token);
    }

    public IEnumerable<ApplicationUser> GetAll()
    {
        return _users;
    }

    public ApplicationUser GetById(int id)
    {
        return _users.FirstOrDefault(x => x.Id == id);
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
    
    public static void RegisterService(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }
}