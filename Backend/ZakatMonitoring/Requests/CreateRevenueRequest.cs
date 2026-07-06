namespace API.Requests;

public class CreateRevenueRequest
{
	public int UserId { get; set; }

	public int CurrencyId { get; set; }

	public double Value { get; set; }

	public double ZakatRemaining { get; set; }

	public double ZakatPaid { get; set; } = 0.0;
}
