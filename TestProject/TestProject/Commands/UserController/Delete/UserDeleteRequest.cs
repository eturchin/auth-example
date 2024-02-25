using MediatR;

namespace TestProject.Commands.UserController.Delete;

public class UserDeleteRequest : IRequest<UserDeleteResponse>
{
    public Guid UserId { get; init; }
}