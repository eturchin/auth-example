using FluentValidation;

namespace TestProject.Commands.UserController.GetById;

public class UserGetByIdRequestValidator : AbstractValidator<UserGetByIdRequest>
{
    public UserGetByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Id cannot be null")
            .NotEmpty().WithMessage("Id cannot be empty");
    }
}