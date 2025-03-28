﻿using CAMS.Common.ResponseApi;
using MediatR;

namespace CAMS.Auctions.Application.Commands.Auctions.StartAuction;

/// <summary>
/// Command to start an auction for a given vehicle.
/// </summary>
public class StartAuctionCommand : IRequest<ResponseResult<StartAuctionResponse>>
{
    public Guid VehicleId { get; }
    public decimal StartingBid { get; }

    public StartAuctionCommand(StartAuctionRequest request)
    {
        VehicleId = request.VehicleId;
        StartingBid = request.StartingBid;
    }
}
