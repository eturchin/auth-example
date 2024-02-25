using System.ComponentModel.DataAnnotations;

namespace TestProject.Models;

public class UserRegisterModel
{
    [Required]
    [StringLength(30)]
    public string Name { get; set; }

    [Required]
    [Range(0, 150)]
    public int Age { get; set; }

    [Required]
    [StringLength(255)]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")] 
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    public List<Guid> RoleIds { get; set; }
}