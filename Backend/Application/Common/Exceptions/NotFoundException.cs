using System;

namespace Application.Common.Exceptions;

public class NotFoundException : Exception
{
	public NotFoundException(string entity, Guid id)
		: base($"{entity} with ID {id} not found.")
	{
	}
}
