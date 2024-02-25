using MediatR;
using TestProject.Models;

namespace TestProject.Commands.UserController.GetList;

public class UserGetListRequest : IRequest<UserGetListResponse>
{
    public FilterRequest Filter { get; init; }
}