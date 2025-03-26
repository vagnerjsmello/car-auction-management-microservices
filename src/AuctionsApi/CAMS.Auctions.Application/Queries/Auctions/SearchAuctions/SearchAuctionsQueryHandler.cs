using CAMS.Auctions.Domain.Repositories;
using CAMS.Common.ResponseApi;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CAMS.Auctions.Application.Queries.Auctions.SearchAuctions;

/// <summary>
/// Handles the SearchAuctionsQuery.
/// </summary>
public class SearchAuctionsQueryHandler : IRequestHandler<SearchAuctionsQuery, ResponseResult<SearchAuctionsResponse>>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly ILogger<SearchAuctionsQueryHandler> _logger;

    public SearchAuctionsQueryHandler(IAuctionRepository auctionRepository, ILogger<SearchAuctionsQueryHandler> logger)
    {
        _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ResponseResult<SearchAuctionsResponse>> Handle(SearchAuctionsQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Searching auctions with filters: Status: {query.Status}, VehicleId: {query.VehicleId}");

        var auctions = await _auctionRepository.SearchAsync(a =>
            (!query.Status.HasValue || a.Status == query.Status.Value)
            && (!query.VehicleId.HasValue || a.VehicleId == query.VehicleId.Value)
            && (!query.AuctionId.HasValue || a.VehicleId == query.AuctionId.Value));

        _logger.LogInformation($"Found {auctions.Count()} auctions.");
        return ResponseResult<SearchAuctionsResponse>.Success(new SearchAuctionsResponse(auctions));
    }
}
