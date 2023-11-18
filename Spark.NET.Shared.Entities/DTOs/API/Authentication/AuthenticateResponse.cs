using Spark.NET.Shared.Entities.Models.User;

namespace Spark.NET.Shared.Entities.DTOs.API.Authentication;


public class AuthenticateResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Token { get; set; }


    public AuthenticateResponse(ApplicationUser applicationUser, string token)
    {
        Id = applicationUser.Id;
        FirstName = applicationUser.FirstName;
        LastName = applicationUser.LastName;
        UserName = applicationUser.UserName;
        Token = token;
    }
}