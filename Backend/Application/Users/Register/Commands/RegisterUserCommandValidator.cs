using FluentValidation;

namespace Application.Users.Register.Commands;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
	public RegisterUserCommandValidator()
	{
		RuleFor((RegisterUserCommand x) => x.Email).NotEmpty().WithMessage("Email is required.").EmailAddress()
			.WithMessage("Invalid email format.");
		RuleFor((RegisterUserCommand x) => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
		RuleFor((RegisterUserCommand x) => x.FirstName).NotEmpty().WithMessage("First name is required.");
		RuleFor((RegisterUserCommand x) => x.LastName).NotEmpty().WithMessage("Last name is required.");
	}
}
