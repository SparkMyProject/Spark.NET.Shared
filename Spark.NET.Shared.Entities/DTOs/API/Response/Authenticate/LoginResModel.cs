using Spark.NET.Shared.Entities.Models.User;

namespace Spark.NET.Shared.Entities.DTOs.API.Response.Authenticate;

public class LoginResModel
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string Token { get; set; }


    public LoginResModel(User user, string token)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Username = user.Username;
        Token = token;
    }
}