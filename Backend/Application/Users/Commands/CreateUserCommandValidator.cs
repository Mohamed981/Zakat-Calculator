using FluentValidation;

namespace Application.Users.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
	public CreateUserCommandValidator()
	{
		RuleFor((CreateUserCommand x) => x.Name).NotEmpty().WithMessage("Name is required.").MaximumLength(200)
			.WithMessage("Name must not exceed 200 characters.");
	}
}
