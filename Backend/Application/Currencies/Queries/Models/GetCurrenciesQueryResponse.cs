namespace Application.Currencies.Queries.Models;

public sealed class GetCurrenciesQueryResponse
{
	public int CurrencyId { get; set; }

	public string Code { get; set; }

	public string Name { get; set; }
}
