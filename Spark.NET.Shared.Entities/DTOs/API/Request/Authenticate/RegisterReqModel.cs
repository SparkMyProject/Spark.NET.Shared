using System.ComponentModel.DataAnnotations;

namespace Spark.NET.Shared.Entities.DTOs.API.Request.Authenticate;

public class RegisterReqModel
{
    [Required(ErrorMessage = "An Email is required.")]
    public string Email { get; set; } = "not@required.com";

    [Required(ErrorMessage = "A Username is required.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "A Password is required.")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "A FirstName is required.")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "A Lastname is required.")]
    public string? LastName { get; set; }

    public DateOnly DateOfBirth { get; set; }
    public string   Address     { get; set; } = "";
}