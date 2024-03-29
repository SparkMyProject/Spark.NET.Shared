using System.ComponentModel.DataAnnotations;

namespace Spark.NET.Shared.Entities.DTOs.API.Authentication;

public class AuthenticateRequest
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}