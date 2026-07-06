namespace API.Requests;

public class CreateTransactionRequest
{
	public int revenueId { get; set; }

	public int currencyId { get; set; }

	public int userId { get; set; }

	public double value { get; set; }
}
