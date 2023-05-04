using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Spark.NET.Shared.Entities.Models.User;

public class User
{
    [Key] public int Id { get; set; }
    [Required] public DateTime CreatedAt = DateTime.Now;

    [Required(ErrorMessage = "An Email is required.")]
    public string Email { get; set; } = "not@required.com";

    [Required(ErrorMessage = "A UserName is required.")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "A Password is required.")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "A FirstName is required.")]
    [DisplayName("FirstName")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "A Lastname is required.")]
    [DisplayName("LastName")]
    public string? LastName { get; set; }

    [DisplayName("Date of Birth")] public DateOnly DateOfBirth { get; set; }

    public string Address { get; set; }
}