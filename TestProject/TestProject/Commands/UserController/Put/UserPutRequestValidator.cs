using FluentValidation;

namespace TestProject.Commands.UserController.Put;

public class UserPutRequestValidator : AbstractValidator<UserPutRequest>
{
    public UserPutRequestValidator()
    {
        RuleFor(x => x.User.Id)
            .NotNull().WithMessage("Id cannot be null")
            .NotEmpty().WithMessage("Id cannot be empty");
        RuleFor(x => x.User.Age)
            .NotNull().WithMessage("Age cannot be null")
            .NotEmpty().WithMessage("Age cannot be empty");
        RuleFor(x => x.User.Name)
            .NotNull().WithMessage("Name cannot be null")
            .NotEmpty().WithMessage("Name cannot be empty");
        RuleFor(x => x.User.Email)
            .NotNull().WithMessage("Email cannot be null")
            .NotEmpty().WithMessage("Email cannot be empty");
    }
}