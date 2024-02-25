using FluentValidation;

namespace TestProject.Commands.UserController.AddRole;

public class UserAddRoleRequestValidator : AbstractValidator<UserAddRoleRequest>
{
    public UserAddRoleRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage("User id cannot be null")
            .NotEmpty().WithMessage("User id cannot be empty");
        RuleFor(x => x.RoleId)
            .NotNull().WithMessage("Role id cannot be null")
            .NotEmpty().WithMessage("Role id cannot be empty");
    }
}