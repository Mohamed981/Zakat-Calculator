using System.Collections.Generic;
using Application.Currencies.Queries.Models;
using MediatR;

namespace Application.Currencies.Queries;

public sealed record GetCurrenciesQuery : IRequest<List<GetCurrenciesQueryResponse>>, IBaseRequest;
