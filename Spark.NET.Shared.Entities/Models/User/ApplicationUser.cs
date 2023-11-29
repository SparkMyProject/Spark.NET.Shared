using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Spark.NET.Shared.Entities.Models.User;

public class ApplicationUser : IdentityUser<string>
{
    [Required] public DateTime CreatedAt = DateTime.Now;
    // [Key]      public string      Id { get; set; }

    [Required(ErrorMessage = "A FirstName is required.")]
    [DisplayName("FirstName")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "A Lastname is required.")]
    [DisplayName("LastName")]
    public string? LastName { get; set; }

    [DisplayName("Date of Birth")] public DateOnly DateOfBirth { get; set; }
    
    public string? Address { get; set; }
}