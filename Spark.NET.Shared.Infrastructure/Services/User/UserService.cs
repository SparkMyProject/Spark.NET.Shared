using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spark.NET.Infrastructure.Services.Authentication;
using Spark.NET.Shared.Entities.DTOs.API.Request.Authenticate;
using Spark.NET.Shared.Entities.DTOs.API.Response.Authenticate;

namespace Spark.NET.Infrastructure.Services.User;

public interface IUserService
{
    LoginResModel? LoginResModel(LoginReqModel model);
    IEnumerable<Shared.Entities.Models.User.User> GetAll();
    Shared.Entities.Models.User.User? GetById(int id);
}

public class UserService : IUserService
{
    public static void RegisterService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
    }
    // users hardcoded for simplicity, store in a db with hashed passwords in production applications
    private List<Shared.Entities.Models.User.User> _users = new List<Shared.Entities.Models.User.User>
    {
        new Shared.Entities.Models.User.User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
    };

    private readonly IJwtUtils _jwtUtils;

    public UserService(IJwtUtils jwtUtils)
    {
        _jwtUtils = jwtUtils;
    }

    public LoginResModel? LoginResModel(LoginReqModel model)
    {
        var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

        // return null if user not found
        if (user == null) return null;

        // authentication successful so generate jwt token
        var token = _jwtUtils.GenerateJwtToken(user);

        return new LoginResModel(user, token);
    }

    public IEnumerable<Shared.Entities.Models.User.User> GetAll()
    {
        return _users;
    }

    public Shared.Entities.Models.User.User? GetById(int id)
    {
        return _users.FirstOrDefault(x => x.Id == id);
    }
}