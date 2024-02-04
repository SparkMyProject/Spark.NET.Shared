namespace Spark.NET.Shared.Entities.DTOs.API.Authentication;

public class DisableAccountRequest
{
    public string Id { get; set; }
    public string Password { get; set; } // Used to verify the user's identity

}