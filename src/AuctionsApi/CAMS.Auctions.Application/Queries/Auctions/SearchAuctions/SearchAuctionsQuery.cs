using CAMS.Auctions.Domain.Enums;
using CAMS.Common.ResponseApi;
using MediatR;

namespace CAMS.Auctions.Application.Queries.Auctions.SearchAuctions;

/// <summary>
/// Query to search auctions based on filter criteria.
/// </summary>
public class SearchAuctionsQuery : IRequest<ResponseResult<SearchAuctionsResponse>>
{
    public SearchAuctionsQuery() { }

    public AuctionStatus? Status { get; set; }
    public Guid? VehicleId { get; set; }
    public Guid? AuctionId { get; set; }
    
}
