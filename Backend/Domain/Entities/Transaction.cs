using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public DateTime? Date { get; set; }

    public string? Value { get; set; }

    public int? CurrencyId { get; set; }

    public int? RevenueId { get; set; }

    public int? UserId { get; set; }

    public virtual Currency? Currency { get; set; }

    public virtual Revenue? Revenue { get; set; }

    public virtual User? User { get; set; }
}
