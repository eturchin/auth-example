using MediatR;

namespace TestProject.Commands.UserController.GetById;

public class UserGetByIdRequest : IRequest<UserGetByIdResponse>
{
    public Guid Id { get; init; }
}