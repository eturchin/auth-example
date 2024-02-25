using MediatR;

namespace TestProject.Commands.UserController.AddRole;

public class UserAddRoleRequest : IRequest<UserAddRoleResponse>
{
    public Guid UserId { get; init; }
    public Guid RoleId { get; init; }
}