using System.ComponentModel.DataAnnotations;

namespace TestProject.Models;

public class UserViewModelSummary
{
    [Required]
    public Guid Id { get; set; }
    
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
}