using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IAppDbContext
{
	DbSet<Currency> Currencies { get; }

	DbSet<Revenue> Revenues { get; }

	DbSet<Transaction> Transactions { get; }

	DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
