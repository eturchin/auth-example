using FluentValidation;

namespace TestProject.Commands.UserController.Delete;

public class UserDeleteRequestValidator : AbstractValidator<UserDeleteRequest>
{
    public UserDeleteRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage("User id cannot be null")
            .NotEmpty().WithMessage("User id cannot be empty");
    }
}