using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Spark.NET.Shared.Entities.Models.User;

public class ApplicationUser : IdentityUser
{
    [Required] public DateTime CreatedAt = DateTime.Now;

    [DisplayName("First Name")] public string FirstName { get; set; }
    [DisplayName("Last Name")] public string LastName { get; set; }
    [DisplayName("Date of Birth")] public DateOnly DateOfBirth { get; set; }

    public string Address { get; set; }
}