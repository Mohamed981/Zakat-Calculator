namespace API.Requests;

public class UpdateRevenueRequest
{
	public int Id { get; set; }

	public int UserId { get; set; }

	public int CurrencyId { get; set; }

	public double Value { get; set; }
}
