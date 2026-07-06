using FluentValidation;

namespace Application.Currencies.Commands.CreateCurrencies;

public class CreateCurrenciesCommandValidator : AbstractValidator<CreateCurrenciesCommand>
{
    public CreateCurrenciesCommandValidator()
    {
        RuleFor(x => x.code)
       .NotEmpty()
       .WithMessage("Code is required.")
       .MaximumLength(6)
       .WithMessage("Code must not exceed 6 characters.");
    }
}
