using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces;

public interface ICacheService
{
	Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken);

	Task SetAsync<T>(string key, T value, TimeSpan? ttl, CancellationToken cancellationToken);

	Task RemoveAsync(string key, CancellationToken cancellationToken);
}
