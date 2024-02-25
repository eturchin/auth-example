using MediatR;
using TestProject.Models;

namespace TestProject.Commands.UserController.Put;

public class UserPutRequest : IRequest<UserPutResponse>
{
    public UserViewModelSummary User { get; init; }
}