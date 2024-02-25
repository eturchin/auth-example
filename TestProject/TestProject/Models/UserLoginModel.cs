using System.ComponentModel.DataAnnotations;

namespace TestProject.Models;

public class UserLoginModel
{
    [Required]
    [StringLength(255)]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}