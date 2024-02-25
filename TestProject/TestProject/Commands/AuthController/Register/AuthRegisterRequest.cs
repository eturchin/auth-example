using MediatR;
using TestProject.Models;

namespace TestProject.Commands.AuthController.Register;

public class AuthRegisterRequest : IRequest<AuthRegisterResponse>
{
    public UserRegisterModel User { get; init; }
}