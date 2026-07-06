using System.Collections.Generic;
using SharedKernel;

namespace Application.Common.Models;

public class Result<T>
{
	public T Results { get; set; }

	public List<Error> Errors { get; set; }

	public List<string> Messages { get; set; }

	public Result()
	{
		Errors = new List<Error>();
		Messages = new List<string>();
	}
}
