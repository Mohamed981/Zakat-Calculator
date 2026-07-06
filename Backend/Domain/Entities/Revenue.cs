using System.Collections.Generic;

namespace Domain.Entities;

public class Revenue
{
	public int RevenueId { get; set; }

	public string? Value { get; set; }

	public string? ZakatIn21k { get; set; }

	public string? ZakatPaid { get; set; }

	public string? ZakatRemaining { get; set; }

	public int? CurrencyId { get; set; }

	public int? UserId { get; set; }

	public virtual Currency? Currency { get; set; }

	public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

	public virtual User? User { get; set; }
}
