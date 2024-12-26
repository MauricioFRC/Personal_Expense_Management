using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class CreateCategoryExpenseValidator : AbstractValidator<CreateCategoryExpenseRequest>
{
    public CreateCategoryExpenseValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .NotNull()
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(70);

        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(50);
    }
}
