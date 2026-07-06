using System.Collections.Generic;

namespace Domain.Entities;

public class Currency
{
	public int CurrencyId { get; set; }

	public string? Code { get; set; }

	public string? Name { get; set; }

	public string? Rates { get; set; }

	public virtual ICollection<Revenue> Revenues { get; set; } = new List<Revenue>();

	public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
