using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Spark.NET.Shared.Entities.Models.User;

public class User
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    // Used to decrypt the password
    [Required]
    public string PasswordSalt { get; set; }
    [Required]
    public DateTime CreatedAt = DateTime.Now;
    
    [DisplayName("First Name")]
    public string FirstName { get; set; }
    [DisplayName("Last Name")]
    public string LastName { get; set; }
    [DisplayName("Date of Birth")]
    public DateOnly DateOfBirth { get; set; }

    public string Address { get; set; }
    [Phone]
    [DisplayName("Phone Number")]
    public string PhoneNumber { get; set; }
}