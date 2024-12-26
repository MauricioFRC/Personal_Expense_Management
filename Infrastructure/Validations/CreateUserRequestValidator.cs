using Core.Request;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Infrastructure.Validations;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    // Minimum eight and maximum 10 characters, at least one uppercase letter,
    // one lowercase letter, one number and one special character
    private readonly string validatePassword = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,10}$";
    private readonly Regex regex;

    public CreateUserRequestValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(60);

        RuleFor(u => u.Email)
            .NotEmpty()
            .NotNull()
            .EmailAddress()
            .MinimumLength(8)
            .MaximumLength(60);

        RuleFor(u => u.Password)
            .NotEmpty()
            .NotNull();
            // .MinimumLength(15)
            // .Must(x => regex!.IsMatch(validatePassword));

        //RuleFor(u => u.Updated_At)
        //    .NotEmpty()
        //    .NotNull();
        //// .LessThan(t => DateTime.UtcNow.AddMinutes(1));

        //RuleFor(u => u.Created_At)
        //    .NotEmpty()
        //    .NotNull();
        //// .LessThan(t => DateTime.UtcNow.AddMinutes(1));
    }
}
