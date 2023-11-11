using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Spark.NET.Shared.Entities.Models.User;

public class ApplicationUser
{
    [Required] public DateTime CreatedAt = DateTime.Now;
    [Key]      public int      Id { get; set; }

    [Required(ErrorMessage = "An Email is required.")]
    public string Email { get; set; } = "not@required.com";

    [Required(ErrorMessage = "A Username is required.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "A Password is required.")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "A FirstName is required.")]
    [DisplayName("FirstName")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "A Lastname is required.")]
    [DisplayName("LastName")]
    public string? LastName { get; set; }

    [DisplayName("Date of Birth")] public DateOnly DateOfBirth { get; set; }

    public string? Address { get; set; }
}