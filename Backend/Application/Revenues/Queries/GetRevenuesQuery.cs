using System.Collections.Generic;
using Application.Revenues.Commands.Models;
using MediatR;

namespace Application.Revenues.Queries;

public sealed record GetRevenuesQuery : IRequest<List<GetRevenuesResponse>>, IBaseRequest;
