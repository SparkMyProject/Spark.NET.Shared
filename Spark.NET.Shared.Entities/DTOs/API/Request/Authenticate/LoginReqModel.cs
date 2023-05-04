using System.ComponentModel.DataAnnotations;

namespace Spark.NET.Shared.Entities.DTOs.API.Request.Authenticate;

public class LoginReqModel
{
    // [Required(ErrorMessage = "An Email is required.")] public string Email { get; set; } = "not@required.com";
    [Required(ErrorMessage = "A Username is required.")] public string? Username { get; set; }
    [Required(ErrorMessage = "A Password is required.")] public string? Password { get; set; }
}