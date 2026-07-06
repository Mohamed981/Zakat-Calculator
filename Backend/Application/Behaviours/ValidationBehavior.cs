using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Behaviours;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (Enumerable.Any(validators))
		{
			ValidationContext<TRequest> context = new ValidationContext<TRequest>(request);
			List<ValidationFailure> failures = (from f in (await Task.WhenAll(validators.Select((IValidator<TRequest> v) => v.ValidateAsync(context, cancellationToken)))).SelectMany((ValidationResult r) => r.Errors)
				where f != null
				select f).ToList();
			if (failures.Any())
			{
				throw new ValidationException(failures);
			}
		}
		return await next(cancellationToken);
	}
}
