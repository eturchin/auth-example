using FluentValidation;

namespace TestProject.Commands.AuthController.Register;

public class AuthRegisterRequestValidator : AbstractValidator<AuthRegisterRequest>
{
    public AuthRegisterRequestValidator()
    {
        RuleFor(x => x.User.Name)
            .NotNull().WithMessage("Name cannot be null")
            .NotEmpty().WithMessage("Name cannot be empty");
        RuleFor(x => x.User.Email)
            .NotNull().WithMessage("Email cannot be null")
            .NotEmpty().WithMessage("Email cannot be empty");
        RuleFor(x => x.User.Age)
            .NotNull().WithMessage("Age cannot be null")
            .NotEmpty().WithMessage("Age cannot be empty");
        RuleFor(x => x.User.Password)
            .NotNull().WithMessage("Password cannot be null")
            .NotEmpty().WithMessage("Password cannot be empty");
        RuleFor(x => x.User.ConfirmPassword)
            .NotNull().WithMessage("ConfirmPassword cannot be null")
            .NotEmpty().WithMessage("ConfirmPassword cannot be empty");
        RuleFor(x => x.User.RoleIds)
            .NotNull().WithMessage("RoleIds cannot be null");
    }
}