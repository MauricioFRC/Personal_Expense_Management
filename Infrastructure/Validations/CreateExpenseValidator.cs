using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseRequest>
{
    public CreateExpenseValidator()
    {
        var actualDate = DateTime.UtcNow;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .NotNull()
            .GreaterThan(0);

        RuleFor(x => x.ExpenseCategoryId)
            .NotEmpty()
            .NotNull()
            .GreaterThan(0)
            .WithMessage("El Id no puede ser negativo");

        RuleFor(x => x.Amount)
            .NotEmpty()
            .NotNull()
            .GreaterThan(0);

        RuleFor(x => x.Date)
            .NotEmpty()
            .NotNull()
            .LessThanOrEqualTo(actualDate)
            .WithMessage("La fecha no puede ser una fecha futura a la actual.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(100);
    }
}
