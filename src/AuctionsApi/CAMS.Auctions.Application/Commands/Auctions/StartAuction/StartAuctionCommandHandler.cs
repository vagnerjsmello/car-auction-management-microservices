using CAMS.Auctions.Domain.Entities;
using CAMS.Auctions.Domain.Repositories;
using CAMS.Common.Exceptions;
using CAMS.Common.ResponseApi;
using CAMS.Infrastructure.Messaging.Events;
using CAMS.Infrastructure.Messaging.Vehicles.CheckAvailability;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace CAMS.Auctions.Application.Commands.Auctions.StartAuction;

/// <summary>
/// Handles the StartAuctionCommand logic.
/// </summary>
public class StartAuctionCommandHandler : IRequestHandler<StartAuctionCommand, ResponseResult<StartAuctionResponse>>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IValidator<StartAuctionCommand> _validator;
    private readonly ILogger<StartAuctionCommandHandler> _logger;
    private readonly IDomainEventPublisher _eventPublisher;
    private readonly IRequestClient<CheckVehicleAvailabilityRequest> _requestClient;

    public StartAuctionCommandHandler(
        IAuctionRepository auctionRepository,
        IValidator<StartAuctionCommand> validator,
        ILogger<StartAuctionCommandHandler> logger,
        IDomainEventPublisher eventPublisher,
        IRequestClient<CheckVehicleAvailabilityRequest> requestClient)
    {
        _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        _requestClient = requestClient ?? throw new ArgumentNullException(nameof(requestClient));
    }

    public async Task<ResponseResult<StartAuctionResponse>> Handle(StartAuctionCommand command, CancellationToken cancellationToken)
    {
        // Validate the command
        ValidationResult validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning($"Validation failed for {command.GetType().Name}: {validationResult.Errors}");
            return ResponseResult<StartAuctionResponse>.Fail(validationResult);
        }

        var existingAuction = await _auctionRepository.GetActiveAuctionByVehicleIdAsync(command.VehicleId);

        if (existingAuction != null)
        {
            var ex = new AuctionAlreadyActiveException(command.VehicleId);
            _logger.LogWarning(ex.Message);  // Or ex.ToString() if want stacktrace
            throw ex;
        }

        var request = new CheckVehicleAvailabilityRequest(command.VehicleId);
        _logger.LogInformation($"Requesting vehicle availability for VehicleId {command.VehicleId}");
        var response = await _requestClient.GetResponse<CheckVehicleAvailabilityResponse>(request);


        var auction = new Auction(command.VehicleId, command.StartingBid);
        var bid = new Bid(command.StartingBid, null);
        auction.Bids.Add(bid);

        await _auctionRepository.AddAsync(auction);
        await _eventPublisher.PublishEventsAsync(auction);

        _logger.LogInformation("Auction {AuctionId} started for vehicle {VehicleId}.", auction.Id, command.VehicleId);

        return ResponseResult<StartAuctionResponse>.Success(new StartAuctionResponse(auction.Id, command.VehicleId, bid.BidderId, command.StartingBid));
    }
}
