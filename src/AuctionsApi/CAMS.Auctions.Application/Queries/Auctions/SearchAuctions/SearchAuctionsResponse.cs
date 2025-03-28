﻿using CAMS.Auctions.Domain.Entities;

namespace CAMS.Auctions.Application.Queries.Auctions.SearchAuctions;

/// <summary>
/// Response containing a list of auctions matching search criteria.
/// </summary>
public class SearchAuctionsResponse
{
    public IEnumerable<Auction> Auctions { get; }

    public SearchAuctionsResponse(IEnumerable<Auction> auctions)
    {
        Auctions = auctions;
    }
}
