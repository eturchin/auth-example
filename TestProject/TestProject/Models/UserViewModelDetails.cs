namespace TestProject.Models;

public class UserViewModelDetails : UserViewModelSummary
{
    public virtual ICollection<RoleViewModelSummary> Roles { get; set; } = new List<RoleViewModelSummary>();
}