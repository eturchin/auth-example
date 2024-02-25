using System.ComponentModel.DataAnnotations;

public class Role
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(35)]
    public string Name { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}