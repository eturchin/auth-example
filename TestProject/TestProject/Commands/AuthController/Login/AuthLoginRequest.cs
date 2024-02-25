using MediatR;
using TestProject.Models;

namespace TestProject.Commands.AuthController.Login;

public class AuthLoginRequest : IRequest<AuthLoginResponse>
{
    public UserLoginModel User { get; init; }
}