using System.Collections.Generic;

namespace Domain.Entities;

public class User
{
	public int UserId { get; set; }

	public string? Name { get; set; }

	public string? TotalPaidZakatIn21k { get; set; }

	public string? TotalZakatIn21k { get; set; }

	public string? ZakatDeadLine { get; set; }

	public string? Email { get; set; }

	public string? Password { get; set; }

	public string? Provider { get; set; }

	public string? ExternalId { get; set; }

	public virtual ICollection<Revenue> Revenues { get; set; } = new List<Revenue>();

	public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
