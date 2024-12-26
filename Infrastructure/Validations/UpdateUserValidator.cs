using Core.DTOs.User;
using FluentValidation;

namespace Infrastructure.Validations;

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .MinimumLength(1)
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .EmailAddress();

        RuleFor(x => x.IsDeleted)
            .NotEmpty()
            .NotNull();
    }
}
